using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ScreenOverwriterServer.Models;
using ScreenOverwriterServer.Services.Database.Interfaces;

namespace ScreenOverwriterServer.Controllers
{
    public class ThreadController : Controller
    {
        private readonly IThreadCreator _threadCreator;

        public ThreadController(IThreadCreator threadCreator)
        {
            _threadCreator = threadCreator;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewThread([FromForm] NewThreadRequestModel newThreadRequest)
        {
            var thread = await _threadCreator.CreateThreadAsync(newThreadRequest.Title, newThreadRequest.BeginningDate);

            string hostUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            var threadViewModel = new ThreadViewModel(thread.ThreadId, thread.ThreadTitle, thread.BeginningDate, hostUrl);

            return this.View("NewThread", threadViewModel);
        }
    }
}
