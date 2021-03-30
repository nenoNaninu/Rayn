using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Rayn.Models;
using Rayn.Models.Requests;
using Rayn.Services.Database.Interfaces;

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
        public async Task<IActionResult> CreateNewThread([FromForm] NewThreadRequest newThreadRequest)
        {
            var thread = await _threadCreator.CreateThreadAsync(newThreadRequest.Title, newThreadRequest.BeginningDate);

            return this.RedirectToAction(nameof(this.Index), new RouteValueDictionary { { "threadId", thread.ThreadId.ToString() } });
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

            var threadViewModel = new ThreadViewModel(thread.ThreadId, thread.ThreadTitle, thread.BeginningDate, HttpContext.Request.Host.Value, thread.OwnerId);

            return this.View(threadViewModel);
        }

        public IActionResult Error()
        {
            return this.View();
        }
    }
}
