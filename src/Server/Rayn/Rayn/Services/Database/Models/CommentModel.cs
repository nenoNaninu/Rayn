using System;

namespace Rayn.Services.Database.Models
{
    public class CommentModel
    {
        public int Id { get; }
        public Guid ThreadId { get; }
        public DateTime WrittenTime { get; }
        public byte[] Message { get; }

        public CommentModel(int id, Guid threadId, DateTime writtenTime, byte[] message)
        {
            Id = id;
            ThreadId = threadId;
            WrittenTime = writtenTime;
            Message = message;
        }
    }
}