using System;
using System.Text;

namespace Rayn.Services.Models
{
    public class CommentModel
    {
        public int Id { get; init; }
        public Guid ThreadId { get; init; }
        public DateTime WrittenTime { get; init; }
        public string Message { get; init; } = default!;

        public byte[] MessageBytes() => Encoding.UTF8.GetBytes(this.Message);
    }
}