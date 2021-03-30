using System;

namespace Rayn.Models.Requests
{
    public class ThreadCreateRequest
    {
        public string Title { get; set; }
        public DateTime BeginningDate { get; set; }
    }
}