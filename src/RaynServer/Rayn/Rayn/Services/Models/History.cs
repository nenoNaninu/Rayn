using System;

namespace Rayn.Services.Models
{
    public class History
    {
        public string OwnerUrl { get; }
        public string ShareUrl { get; }
        public string Title { get; }
        public DateTime DateTime { get; }

        public History(string ownerUrl, string shareUrl, string title, DateTime dateTime)
        {
            OwnerUrl = ownerUrl;
            ShareUrl = shareUrl;
            Title = title;
            DateTime = dateTime;
        }
    }
}