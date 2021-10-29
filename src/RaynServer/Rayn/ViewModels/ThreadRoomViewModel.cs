using System;
using Rayn.Services.Realtime.Hubs;

namespace Rayn.ViewModels
{
    public class ThreadRoomViewModel
    {
        public string ThreadTitle { get; }
        public Guid ThreadId { get; }

        public string ThreadRoomHubUrl() => ThreadRoomHub.Path;

        public ThreadRoomViewModel(string threadTitle, Guid threadId)
        {
            ThreadTitle = threadTitle;
            ThreadId = threadId;
        }
    }
}
