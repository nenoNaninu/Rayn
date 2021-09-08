using System;

namespace Rayn.Services.Models
{
    public class ThreadModel
    {
        public Guid ThreadId { get; init; }
        public Guid OwnerId { get; init; }
        public string ThreadTitle { get; init; } = string.Empty;
        public DateTime BeginningDate { get; init; }
        public TimeSpan DateOffset { get; init; }
        public DateTime CreatedDate { get; init; }
        public Guid? AuthorId { get; init; } 
    }
}