using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RxWebSocket;
using RxWebSocket.Extensions;
using RxWebSocket.Senders;

namespace ScreenOverwriterServer.Services.Realtime
{
    public class ThreadRoomMiddleware
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
            if (!context.WebSockets.IsWebSocketRequest) return;

            var query = context.Request.Query;
            
            string threadIdString = query["threadId"].ToString();
            var threadId = Guid.Parse(threadIdString);

            var socket = await context.WebSockets.AcceptWebSocketAsync();

            using var rxSocket = new WebSocketClient(socket, new BinaryOnlySender(), logger: _logger.AsWebSocketLogger());

            await rxSocket.ConnectAsync();

            // 接続したら、threadに対応するthreadRoomを取得して、そこに登録。

            var threadRoom = await _threadRoomStore.FetchThreadRoomAsync(threadId);

            await threadRoom.AddAsync(rxSocket);

            await rxSocket.WaitUntilCloseAsync();
        }
    }
}