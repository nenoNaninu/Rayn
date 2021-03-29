using System;

namespace ScreenOverwriterServer.Models
{
    public class ThreadViewModel
    {
        public Guid ThreadId { get; }
        public string ThreadTitle { get; }
        public DateTime BeginningDate { get; }
        public string HostUrl { get; }

        public string ThreadUrl() => $"{HostUrl}/ThreadRoom/?threadId={ThreadId.ToString()}";

        public ThreadViewModel(Guid threadId, string threadTitle, DateTime beginningDate, string hostUrl)
        {
            ThreadId = threadId;
            ThreadTitle = threadTitle;
            BeginningDate = beginningDate;
            HostUrl = hostUrl;
        }
    }
}