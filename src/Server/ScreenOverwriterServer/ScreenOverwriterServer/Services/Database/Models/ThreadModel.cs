using System;

namespace ScreenOverwriterServer.Services.Database.Models
{
    public class ThreadModel
    {
        public Guid ThreadId { get; }
        public string ThreadTitle { get; }
        public DateTime BeginningDate { get; }

        public ThreadModel(Guid threadId, DateTime beginningDate, string threadTitle)
        {
            ThreadId = threadId;
            BeginningDate = beginningDate;
            ThreadTitle = threadTitle;
        }
    }
}