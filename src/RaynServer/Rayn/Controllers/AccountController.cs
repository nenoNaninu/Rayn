using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;

namespace Rayn.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRegister _accountRegister;
        private readonly IGoogleAccountReader _googleAccountReader;
        private readonly IGoogleAccountRegister _googleAccountRegister;
        public AccountController(
            IGoogleAccountReader googleAccountReader,
            IAccountRegister accountRegister,
            IGoogleAccountRegister googleAccountRegister)
        {
            _accountRegister = accountRegister;
            _googleAccountReader = googleAccountReader;
            _googleAccountRegister = googleAccountRegister;
        }

        public IActionResult Login()
        {
            return this.View();
        }

        public IActionResult GoogleAuth()
        {
            var prop = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(this.GoogleAuthCallback))
            };

            return this.Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }

        private async ValueTask<GoogleAccount> RegisterGoogleAccount(string identifier)
        {
            var email = this.User.FindFirst(ClaimTypes.Email)?.Value!;

            var userId = Guid.NewGuid();

            var googleAccount = new GoogleAccount()
            {
                Identifier = identifier,
                Email = email,
                UserId = userId
            };

            var account = new Account()
            {
                UserId = userId,
                Email = email,
                LinkToGoogle = true
            };

            await _googleAccountRegister.RegisterAsync(googleAccount);
            await _accountRegister.RegisterAsync(account);

            return googleAccount;
        }

        [Authorize]
        public async Task<IActionResult> GoogleAuthCallback()
        {
            var identifierClaim = this.User.FindAll(ClaimTypes.NameIdentifier)
                .FirstOrDefault(x => x.Issuer == "Google");

            if (identifierClaim is null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            var identifier = identifierClaim.Value;

            var account = await _googleAccountReader.SearchAsync(identifier) 
                                    ?? await this.RegisterGoogleAccount(identifier);

            var principal = this.User;
            var identity = new ClaimsIdentity();
            var claim = new Claim(
                ClaimTypes.NameIdentifier, 
                account.UserId.ToString(), 
                null, 
                "Rayn");

            identity.AddClaim(claim);
            principal.AddIdentity(identity);

            await this.HttpContext.SignInAsync(principal);

            return this.RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync();
            return this.RedirectToAction("Index", "Home");
        }
    }
}
