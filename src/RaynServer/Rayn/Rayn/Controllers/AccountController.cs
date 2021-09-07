using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Database.Models;

namespace Rayn.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountReader _accountReader;
        private readonly IAccountRegister _accountRegister;
        private readonly IGoogleAccountReader _googleAccountReader;
        private readonly IGoogleAccountRegister _googleAccountRegister;

        public AccountController(IAccountReader accountReader, IGoogleAccountReader googleAccountReader, IAccountRegister accountRegister, IGoogleAccountRegister googleAccountRegister)
        {
            _accountReader = accountReader;
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

        [Authorize]
        public async Task<IActionResult> GoogleAuthCallback()
        {
            var identifier = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (identifier is null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            var registeredAccount = await _googleAccountReader.SearchAsync(identifier);

            if (registeredAccount is null)
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

                this.HttpContext.Response.Cookies.Append("UserId", userId.ToString());
            }

            return this.RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return this.RedirectToAction("Index", "Home");
        }
    }
}
