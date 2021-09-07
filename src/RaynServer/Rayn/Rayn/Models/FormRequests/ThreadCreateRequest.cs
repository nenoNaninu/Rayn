using System;

namespace Rayn.Models.FormRequests
{
    public class ThreadCreateRequest
    {
        public string? Title { get; init; }
        public DateTime BeginningDate { get; init; }
    }
}