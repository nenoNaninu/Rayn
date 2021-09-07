using System;

namespace Rayn.Services.Database.Models
{
    public class GoogleAccount
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string Identifier { get; }

        public GoogleAccount(Guid userId, string email, string identifier)
        {
            this.UserId = userId;
            this.Email = email;
            this.Identifier = identifier;
        }
    }
}