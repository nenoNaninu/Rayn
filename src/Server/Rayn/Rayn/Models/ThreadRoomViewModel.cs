using System;

namespace Rayn.Models
{
    public class ThreadRoomViewModel
    {
        public string ThreadTitle { get; }
        public Guid ThreadId { get; }

        private readonly string _hostDomain;

        public string RealTimeUrl() => $"wss://{_hostDomain}/Realtime/?threadId={ThreadId.ToString()}";

        public ThreadRoomViewModel(string threadTitle, Guid threadId, string hostDomain)
        {
            ThreadTitle = threadTitle;
            ThreadId = threadId;
            _hostDomain = hostDomain;
        }
    }
}