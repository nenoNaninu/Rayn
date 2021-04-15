using System;
using Rayn.Services.Url;

namespace Rayn.Models
{
    public class ThreadRoomViewModel
    {
        public string ThreadTitle { get; }
        public Guid ThreadId { get; }

        private readonly string _hostDomain;

        public string ThreadRoomHubUrl() => UrlUtility.ThreadRoomHubUrl(_hostDomain);

        public ThreadRoomViewModel(string threadTitle, Guid threadId, string hostDomain)
        {
            ThreadTitle = threadTitle;
            ThreadId = threadId;
            _hostDomain = hostDomain;
        }
    }
}