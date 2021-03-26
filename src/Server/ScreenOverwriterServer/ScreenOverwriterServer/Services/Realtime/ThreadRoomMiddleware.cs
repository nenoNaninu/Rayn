﻿using System;
using System.Buffers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RxWebSocket;
using RxWebSocket.Extensions;

namespace ScreenOverwriterServer.Services.Realtime
{
    public class ThreadRoomMiddleware
    {
        // 短絡させるのでいらない。
        //private readonly RequestDelegate _next;

        private readonly ILogger<ThreadRoomMiddleware> _logger;
        private readonly IThreadRoomStore _threadRoomStore;

        public ThreadRoomMiddleware(ILogger<ThreadRoomMiddleware> logger, IThreadRoomStore threadRoomStore)
        {
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest) return;

            var query = context.Request.Query;
            
            string threadIdString = query["threadId"].ToString();
            var guid = Guid.Parse(threadIdString);

            var socket = await context.WebSockets.AcceptWebSocketAsync();

            using var rxSocket = new WebSocketClient(socket, logger: _logger.AsWebSocketLogger());

            await rxSocket.ConnectAsync();

            

            //If you do not wait here, the connection will be disconnected.
            await rxSocket.WaitUntilCloseAsync();
        }
    }
}