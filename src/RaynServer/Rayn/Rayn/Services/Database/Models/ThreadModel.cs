using System;

namespace Rayn.Services.Database.Models
{
    public class ThreadModel
    {
        public Guid ThreadId { get; set; }
        public Guid OwnerId { get; set; }
        public string ThreadTitle { get; set; }
        public DateTime BeginningDate { get; set; }

        public ThreadModel(Guid threadId, Guid ownerId, string threadTitle, DateTime beginningDate)
        {
            ThreadId = threadId;
            BeginningDate = beginningDate;
            ThreadTitle = threadTitle;
            OwnerId = ownerId;
        }

        // ORM用
        public ThreadModel()
        {
        }
    }
}