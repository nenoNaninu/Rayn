using System;

namespace Rayn.Services.Models
{
    public class GoogleAccount
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = default!;
        public string Identifier { get; init; } = default!;
    }
}