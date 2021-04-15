using System;

namespace Rayn.Services.Url
{
    // 散らばらないようにまとめてるけど、より良い方法なんかありそうではある。
    public static class UrlUtility
    {
        public static string ThreadUrl(string hostDomain, Guid threadId)
            => $"https://{hostDomain}/ThreadRoom/?threadId={threadId.ToString()}";
        public static string StreamerUrl(string hostDomain, Guid threadId, Guid ownerId)
            => $"https://{hostDomain}/ThreadRoom/Streamer?threadId={threadId.ToString()}&ownerId={ownerId.ToString()}";
        public static string WebsSocketRealtimeThreadRoomUrl(string hostDomain, Guid threadId, Guid ownerId)
            => $"wss://{hostDomain}/Realtime/?threadId={threadId.ToString()}&ownerId={ownerId.ToString()}";
        public static string PollingMessageUrl(string hostDomain, Guid threadId, Guid ownerId)
            => $"https://{hostDomain}/ThreadRoom/FetchMessages/?threadId={threadId.ToString()}&ownerId={ownerId.ToString()}";
        public static string ThreadRoomHubUrl(string hostDomain) 
            => $"https://{hostDomain}/Realtime/ThreadRoom";
    }
}