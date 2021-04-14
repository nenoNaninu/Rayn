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
        public async Task<ActionResult<StreamerConnectionResponse>> Streamer(string threadId, string ownerId, string method)
        {
            if (threadId == null || ownerId == null || !Guid.TryParse(threadId, out var threadGuid) || !Guid.TryParse(ownerId, out var ownerGuid))
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

            var host = HttpContext.Request.Host.Value;



            if (method == null || method != "polling")
            {
                // websocket
                return new StreamerConnectionResponse(StreamerConnectionRequestStatus.Ok, UrlUtility.WebsSocketRealtimeThreadRoomUrl(host, threadGuid, ownerGuid));
            }

            // ポーリングの時は接続先を要求された時点でroomに仲間入りさせておく。
            var connection = _pollingUserConnectionCreator.Create(threadGuid, ownerGuid);

            var threadRoom = await _threadRoomStore.FetchThreadRoomAsync(threadGuid);
            await threadRoom.AddAsync(connection);

            return new StreamerConnectionResponse(StreamerConnectionRequestStatus.Ok, UrlUtility.PollingMessageUrl(host, threadGuid, ownerGuid));
        }


        [HttpGet]
        public IReadOnlyList<string> FetchMessages(string threadId, string ownerId)
        {
            // とりあえずownerIdだけで引っ張ってくる。
            if (threadId == null || ownerId == null || !Guid.TryParse(threadId, out var threadGuid) || !Guid.TryParse(ownerId, out var ownerGuid))
            {
                return Array.Empty<string>();
            }

            var connection = _pollingUserConnectionStore.FetchPollingUserConnection(ownerGuid);

            return connection?.ReadSentMessages() ?? Array.Empty<string>();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return this.View();
        }
    }
}
