using System;

namespace ScreenOverwriterServer.Services.Database.Models
{
    public class CommentModel
    {
        public int Id { get; }
        public Guid ThreadId { get; }
        public DateTime WrittenTime { get; }
        public string Text { get; }

        public CommentModel(int id, Guid threadId, DateTime writtenTime, string text)
        {
            Id = id;
            ThreadId = threadId;
            WrittenTime = writtenTime;
            Text = text;
        }
    }
}