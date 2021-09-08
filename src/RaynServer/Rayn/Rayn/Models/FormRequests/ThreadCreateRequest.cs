using System;

namespace Rayn.Models.FormRequests
{
    public class ThreadCreateRequest
    {
        public string Title { get; init; } = default!;
        public DateTime BeginningDate { get; init; }
        public int DateOffset { get; init; }
    }
}