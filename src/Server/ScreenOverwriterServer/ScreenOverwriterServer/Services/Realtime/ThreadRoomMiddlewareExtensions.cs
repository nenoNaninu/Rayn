using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ScreenOverwriterServer.Services.Realtime
{
    public static class ThreadRoomMiddlewareExtensions
    {
        public static IApplicationBuilder MapThreadRoomMiddleware(this IApplicationBuilder appBuilder, PathString path, IThreadRoomStore threadRoomStore, ILogger<ThreadRoomMiddleware> logger)
        {
            return appBuilder.Map(path, app => app.UseMiddleware<ThreadRoomMiddleware>(threadRoomStore, logger));
        }
    }
}