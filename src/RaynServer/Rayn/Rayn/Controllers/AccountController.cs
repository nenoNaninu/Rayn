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

namespace Rayn.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountReader _accountReader;
        private readonly IAccountRegister _accountRegister;
        private readonly IGoogleAccountReader _googleAccountReader;

        public AccountController(IAccountReader accountReader, IGoogleAccountReader googleAccountReader ,IAccountRegister accountRegister)
        {
            _accountReader = accountReader;
            _accountRegister = accountRegister;
            _googleAccountReader = googleAccountReader;
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
        public IActionResult GoogleAuthCallback()
        {
            var identifier = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (identifier is null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            var account = _googleAccountReader.Search(identifier);

            if (account is null)
            {
                var email = this.User.FindFirst(ClaimTypes.Email)?.Value;
                 
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
