using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Rayn.Services.Database.Abstractions;
using Rayn.Services.Models;
using Rayn.Services.Requests;
using Rayn.Services.Url;
using Rayn.ViewModels;

namespace Rayn.Controllers;

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
            BeginningDate = threadCreateRequest.BeginningDate - dateOffset,
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
        if (threadId is null)
        {
            return this.RedirectToAction(nameof(this.Error));
        }

        if (!Guid.TryParse(threadId, out var threadGuid))
        {
            return this.RedirectToAction(nameof(this.Error));
        }

        var thread = await _threadDbReader.SearchThreadModelAsync(threadGuid);

        if (thread is null)
        {
            return this.RedirectToAction(nameof(this.Error));
        }

        var host = this.HttpContext.Request.Host.Value;

        var threadUrl = UrlUtility.ThreadUrl(host, thread.ThreadId);
        var streamerUrl = UrlUtility.StreamerUrl(host, thread.ThreadId, thread.OwnerId);

        var threadViewModel = new ThreadViewModel(
            thread.ThreadTitle,
            thread.BeginningDate + thread.DateOffset,
            threadUrl,
            streamerUrl);

        return this.View(threadViewModel);
    }

    public IActionResult Error()
    {
        return this.View();
    }
}
