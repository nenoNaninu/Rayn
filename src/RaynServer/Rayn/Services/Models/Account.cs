using System;

namespace Rayn.Services.Models
{
    public class Account
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = default!;
        public bool LinkToGoogle { get; init; }
    }
}
