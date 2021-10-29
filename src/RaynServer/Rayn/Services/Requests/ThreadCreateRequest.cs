using System;

namespace Rayn.Services.Requests
{
    public class ThreadCreateRequest
    {
        public string Title { get; init; } = default!;
        public DateTime BeginningDate { get; init; }
        public int DateOffset { get; init; }
    }
}
