using System;

namespace Rayn.ViewModels
{
    public class ThreadViewModel
    {
        public string ThreadTitle { get; }
        public DateTime BeginningDate { get; }
        public string ThreadUrl { get; }
        public string StreamerUrl { get; }

        public ThreadViewModel(
            string threadTitle,
            DateTime beginningDate,
            string threadUrl,
            string streamerUrl)
        {
            ThreadTitle = threadTitle;
            BeginningDate = beginningDate;
            ThreadUrl = threadUrl;
            StreamerUrl = streamerUrl;
        }
    }
}