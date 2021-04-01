using System;
using System.Text;

namespace Rayn.Services.Database.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public Guid ThreadId { get; set; }
        public DateTime WrittenTime { get; set; }
        public string Message { get; set; }

        public byte[] MessageBytes() => Encoding.UTF8.GetBytes(this.Message);

        public CommentModel(int id, Guid threadId, DateTime writtenTime, string message)
        {
            Id = id;
            ThreadId = threadId;
            WrittenTime = writtenTime;
            Message = message;
        }

        // ORM用
        public CommentModel()
        {
        }
    }
}