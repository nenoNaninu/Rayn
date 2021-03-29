using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScreenOverwriterServer.Models;
using ScreenOverwriterServer.Services.Database.Interfaces;

namespace ScreenOverwriterServer.Controllers
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
        public IActionResult Error()
        {
            return this.View();
        }
    }
}
