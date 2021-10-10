using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;
using Rayn.Services.Url;
using Rayn.ViewModels;

namespace Rayn.Controllers
{
    [Authorize]
    public class HistoryController : Controller
    {
        private readonly IThreadDbReader _threadDbReader;

        public HistoryController(IThreadDbReader threadDbReader)
        {
            _threadDbReader = threadDbReader;
        }

        public async Task<IActionResult> Index()
        {
            var claim = this.User.FindAll(ClaimTypes.NameIdentifier)
                .FirstOrDefault(x => x.Issuer == "Rayn");

            if (claim is null)
            {
                return this.RedirectToAction("Error", "Home");
            }

            var userId = Guid.Parse(claim.Value);
            var threads = await _threadDbReader.SearchThreadByUserId(userId);

            var host = this.HttpContext.Request.Host.Value;

            var histories = threads
                .Select(x =>
                {
                    var ownerUrl = UrlUtility.StreamerUrl(host, x.ThreadId, x.OwnerId);
                    var shareUrl = UrlUtility.ThreadUrl(host, x.ThreadId);
                    var title = x.ThreadTitle;
                    var scheduledDateTime = x.BeginningDate + x.DateOffset;
                    return new History(ownerUrl, shareUrl, title, scheduledDateTime);
                });

            return this.View(new HistoryViewModel(histories));
        }
    }
}
