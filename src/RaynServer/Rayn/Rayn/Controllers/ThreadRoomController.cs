using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rayn.Models;
using Rayn.Models.ApiResponse;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Realtime;
using Rayn.Services.Realtime.Interfaces;
using Rayn.Services.Url;

namespace Rayn.Controllers
{
    public class ThreadRoomController : Controller
    {
        private readonly IThreadDbReader _threadDbReader;
        //private readonly IPollingUserConnectionStore _pollingUserConnectionStore;
        //private readonly IThreadRoomStore _threadRoomStore;
        //private readonly IPollingUserConnectionCreator _pollingUserConnectionCreator;

        public ThreadRoomController(IThreadDbReader threadDbReader)
            //, IPollingUserConnectionStore pollingUserConnectionStore, IThreadRoomStore threadRoomStore, IPollingUserConnectionCreator pollingUserConnectionCreator)
        {
            _threadDbReader = threadDbReader;
            //_pollingUserConnectionStore = pollingUserConnectionStore;
            //_threadRoomStore = threadRoomStore;
            //_pollingUserConnectionCreator = pollingUserConnectionCreator;
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
            if (threadId == null || ownerId == null || !Guid.TryParse(threadId, out var threadGuid) || !Guid.TryParse(ownerId, out var ownerGuid))
            {
                return new StreamerConnectionResponse(StreamerConnectionRequestStatus.BadRequest, string.Empty, Guid.Empty);
            }

            var thread = await _threadDbReader.SearchThreadModelAsync(threadGuid);

            if (thread == null)
            {
                return new StreamerConnectionResponse(StreamerConnectionRequestStatus.ThreadRoomNotExist, string.Empty, Guid.Empty);
            }

            if (ownerGuid != thread.OwnerId)
            {
                return new StreamerConnectionResponse(StreamerConnectionRequestStatus.BadRequest, string.Empty, Guid.Empty);
            }

            var host = HttpContext.Request.Host.Value;

            var threadRoomHubUrl = UrlUtility.ThreadRoomHubUrl(host);

            return new StreamerConnectionResponse(StreamerConnectionRequestStatus.Ok, threadRoomHubUrl, threadGuid);
        }

        [HttpGet]
        public IActionResult Error()
        {
            return this.View();
        }
    }
}
