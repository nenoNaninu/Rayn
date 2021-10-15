using System;

namespace Rayn.Services.Models
{
    public class ThreadModel
    {
        public Guid ThreadId { get; init; }
        public Guid OwnerId { get; init; }
        public string ThreadTitle { get; init; } = string.Empty;

        /// <summary>
        /// UTC
        /// </summary>
        public DateTime BeginningDate { get; init; }

        /// <summary>
        /// Offset from UTC.
        /// For example, if tokyo time zone, DateOffset represents TimeSpan.FromHours(9).
        /// </summary>
        public TimeSpan DateOffset { get; init; }
        public DateTime CreatedDate { get; init; }
        public Guid? AuthorId { get; init; }
    }
}