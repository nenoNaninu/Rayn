using System;

namespace Rayn.Models.Requests
{
    public class NewThreadRequest
    {
        public string Title { get; set; }
        public DateTime BeginningDate { get; set; }
    }
}