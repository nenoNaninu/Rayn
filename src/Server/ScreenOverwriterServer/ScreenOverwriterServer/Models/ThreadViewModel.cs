using System;
using ScreenOverwriterServer.Services.Url;

namespace ScreenOverwriterServer.Models
{
    public class ThreadViewModel
    {
        public Guid ThreadId { get; }
        public Guid OwnerId { get; }
        public string ThreadTitle { get; }
        public DateTime BeginningDate { get; }
        public string HostDomain { get; }

        public string ThreadUrl() => UrlUtility.ThreadUrl(HostDomain, ThreadId);

        public string StreamerUrl() => UrlUtility.StreamerUrl(HostDomain, ThreadId, OwnerId);

        public ThreadViewModel(Guid threadId, string threadTitle, DateTime beginningDate, string hostDomain, Guid ownerId)
        {
            ThreadId = threadId;
            ThreadTitle = threadTitle;
            BeginningDate = beginningDate;
            HostDomain = hostDomain;
            OwnerId = ownerId;
        }
    }
}