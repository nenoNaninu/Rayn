using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rayn.Services.Realtime.Interfaces;
using RxWebSocket;
using RxWebSocket.Extensions;
using RxWebSocket.Senders;

namespace Rayn.Services.Realtime
{
    public sealed class ThreadRoomMiddleware
    {
        // 短絡させるのでいらない。
        //private readonly RequestDelegate _next;

        private readonly ILogger<ThreadRoomMiddleware> _logger;
        private readonly IThreadRoomStore _threadRoomStore;

        public ThreadRoomMiddleware(RequestDelegate next, IThreadRoomStore threadRoomStore, ILogger<ThreadRoomMiddleware> logger)
        {
            _logger = logger;
            _threadRoomStore = threadRoomStore;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (!context.WebSockets.IsWebSocketRequest) return;

                var query = context.Request.Query;

                string threadIdString = query["threadId"].ToString();

                var socket = await context.WebSockets.AcceptWebSocketAsync();

                if (Guid.TryParse(threadIdString, out var threadId))
                {
                    using var client = new WebSocketClient(socket, new BinaryOnlySender(), logger: _logger.AsWebSocketLogger());

                    await client.ConnectAsync();

                    // 接続したら、threadに対応するthreadRoomを取得して、そこに登録。

                    var threadRoom = await _threadRoomStore.FetchThreadRoomAsync(threadId);

                    await threadRoom.AddAsync(client);
                    
                    await client.WaitUntilCloseAsync();
                }
                else
                {
                    var cancellationToken = new CancellationTokenSource();
                    cancellationToken.CancelAfter(TimeSpan.FromSeconds(30).Milliseconds);
                    await socket.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "", cancellationToken.Token);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"[Exception occur in {nameof(ThreadRoomMiddleware)}.{nameof(this.Invoke)}] {e.Message}");
            }
        }
    }
}