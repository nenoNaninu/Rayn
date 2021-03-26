using System;

namespace ScreenOverwriterServer.Services.Database.Models
{
    public class ThreadModel
    {
        public Guid ThreadId { get; }
        public string ThreadTitle { get; }
        public DateTime BeginningTime { get; }

        public ThreadModel(Guid threadId, DateTime beginningTime, string threadTitle)
        {
            ThreadId = threadId;
            BeginningTime = beginningTime;
            ThreadTitle = threadTitle;
        }
    }
}