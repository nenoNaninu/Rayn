using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Rayn.Models;
using Rayn.Models.FormRequests;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Models;

namespace Rayn.Controllers
{
    public class ThreadController : Controller
    {
        private readonly IThreadCreator _threadCreator;
        private readonly IThreadDbReader _threadDbReader;

        public ThreadController(IThreadCreator threadCreator, IThreadDbReader threadDbReader)
        {
            _threadCreator = threadCreator;
            _threadDbReader = threadDbReader;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewThread([FromForm] ThreadCreateRequest threadCreateRequest)
        {
            var claim = this.User.FindAll(ClaimTypes.NameIdentifier)?.Where(x => x.Issuer == "Rayn").FirstOrDefault();
            Guid? userId = claim is not null ? Guid.Parse(claim.Value) : null;

            var dateOffset = TimeSpan.FromMinutes(threadCreateRequest.DateOffset);
            var thread = new ThreadModel()
            {
                ThreadTitle = threadCreateRequest.Title,
                BeginningDate = threadCreateRequest.BeginningDate + dateOffset,
                DateOffset = dateOffset,
                AuthorId = userId,
                CreatedDate = DateTime.UtcNow,
                OwnerId = Guid.NewGuid(),
                ThreadId = Guid.NewGuid()
            };

            await _threadCreator.CreateThreadAsync(thread);

            return this.RedirectToAction(nameof(this.Index), new RouteValueDictionary { { "threadId", thread.ThreadId.ToString() } });
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? threadId)
        {
            if (threadId == null)
            {
                return this.RedirectToAction(nameof(this.Error));
            }

            if (!Guid.TryParse(threadId, out var threadGuid))
            {
                return this.RedirectToAction(nameof(this.Error));
            }

            var thread = await _threadDbReader.SearchThreadModelAsync(threadGuid);

            if (thread == null)
            {
                return this.RedirectToAction(nameof(this.Error));
            }

            var threadUrl = this.ThreadUrl(thread.ThreadId);
            var streamerUrl = this.StreamerUrl(thread.ThreadId, thread.OwnerId);

            var threadViewModel = new ThreadViewModel(
                thread.ThreadTitle,
                thread.BeginningDate - thread.DateOffset,
                threadUrl,
                streamerUrl);

            return this.View(threadViewModel);
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


        public IActionResult Error()
        {
            return this.View();
        }
    }
}
