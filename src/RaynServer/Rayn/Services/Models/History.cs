using System;

namespace Rayn.Services.Models
{
    public class History
    {
        public string OwnerUrl { get; }
        public string ShareUrl { get; }
        public string Title { get; }
        public DateTime ScheduledDateTime { get; }

        public History(string ownerUrl, string shareUrl, string title, DateTime scheduledDateTime)
        {
            OwnerUrl = ownerUrl;
            ShareUrl = shareUrl;
            Title = title;
            ScheduledDateTime = scheduledDateTime;
        }
    }
}