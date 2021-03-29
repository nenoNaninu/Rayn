using System;

namespace ScreenOverwriterServer.Services.Url
{
    public static class UrlUtility
    {
        public static string ThreadUrl(string hostDomain, Guid threadId) => $"https://{hostDomain}/ThreadRoom/?threadId={threadId.ToString()}";
        public static string StreamerUrl(string hostDomain, Guid threadId, Guid ownerId) => $"https://{hostDomain}/ThreadRoom/Streamer?threadId={threadId.ToString()}&ownerId={ownerId.ToString()}";
        public static string RealtimeThreadRoomUrl(string hostDomain, Guid threadId, Guid ownerId) => $"wss://{hostDomain}/Realtime/?threadId={threadId.ToString()}&ownerId={ownerId.ToString()}";
    }
}