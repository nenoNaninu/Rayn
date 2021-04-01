using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Rayn.Models;
using Rayn.Models.ApiResponse;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Url;

namespace Rayn.Controllers
{
    public class ThreadRoomController : Controller
    {
        private readonly IThreadDbReader _threadDbReader;

        public ThreadRoomController(IThreadDbReader threadDbReader)
        {
            _threadDbReader = threadDbReader;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string threadId)
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

            var threadRoomViewModel = new ThreadRoomViewModel(thread.ThreadTitle, thread.ThreadId, HttpContext.Request.Host.Value);

            return this.View(threadRoomViewModel);
        }

        [HttpGet]
        public async Task<ActionResult<StreamerConnectionResponse>> Streamer(string threadId, string ownerId)
        {
            if (threadId == null || !Guid.TryParse(threadId, out var threadGuid) || !Guid.TryParse(ownerId, out var ownerGuid))
            {
                return new StreamerConnectionResponse(StreamerConnectionRequestStatus.BadRequest, "");
            }

            var thread = await _threadDbReader.SearchThreadModelAsync(threadGuid);

            if (thread == null)
            {
                return new StreamerConnectionResponse(StreamerConnectionRequestStatus.ThreadRoomNotExist, "");
            }

            if (ownerGuid != thread.OwnerId)
            {
                return new StreamerConnectionResponse(StreamerConnectionRequestStatus.BadRequest, "");
            }

            return new StreamerConnectionResponse(StreamerConnectionRequestStatus.Ok, UrlUtility.RealtimeThreadRoomUrl(HttpContext.Request.Host.Value, thread.ThreadId, ownerGuid));
        }

        [HttpGet]
        public IActionResult Error()
        {
            return this.View();
        }
    }
}
