using System;

namespace Rayn.Services.Database.Models
{
    public class ThreadModel
    {
        public Guid ThreadId { get; init; }
        public Guid OwnerId { get; init; }
        public string ThreadTitle { get; init; } = default!;
        public DateTime BeginningDate { get; init; }
        public DateTime CreatedDate { get; init; }
        public Guid? AuthorId { get; init; } 
    }
}