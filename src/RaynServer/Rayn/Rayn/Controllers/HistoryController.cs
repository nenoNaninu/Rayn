using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;
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

            var histories = threads.Select(x =>
            {
                var ownerUrl = this.StreamerUrl(x.ThreadId, x.OwnerId);
                var shareUrl = this.ThreadUrl(x.ThreadId);
                var title = x.ThreadTitle;
                var date = x.BeginningDate + x.DateOffset;
                return new History(ownerUrl, shareUrl, title, date);
            });

            return this.View(new HistoryViewModel(histories));
        }

        private string ThreadUrl(Guid threadId)
        {
            var protocol = this.HttpContext.Request.Scheme;

            return this.Url.Action(null, "ThreadRoom",
                new { threadId = threadId.ToString() },
                protocol);
        }

        private string StreamerUrl(Guid threadId, Guid ownerId)
        {
            var protocol = this.HttpContext.Request.Scheme;

            return this.Url.Action(nameof(ThreadRoomController.Streamer), "ThreadRoom",
                new { threadId = threadId.ToString(), ownerId = ownerId.ToString() },
                protocol);
        }
    }
}
