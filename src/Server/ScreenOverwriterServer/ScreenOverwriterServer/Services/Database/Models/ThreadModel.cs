using System;

namespace ScreenOverwriterServer.Services.Database.Models
{
    public class ThreadModel
    {
        public Guid ThreadId { get; }
        public Guid OwnerId { get; }
        public string ThreadTitle { get; }
        public DateTime BeginningDate { get; }

        public ThreadModel(Guid threadId, DateTime beginningDate, string threadTitle, Guid ownerId)
        {
            ThreadId = threadId;
            BeginningDate = beginningDate;
            ThreadTitle = threadTitle;
            OwnerId = ownerId;
        }
    }
}