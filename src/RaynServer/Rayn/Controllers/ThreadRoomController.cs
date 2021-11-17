using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Realtime.Hubs;
using Rayn.Services.Realtime.Interfaces;
using Rayn.Services.Realtime.Models;
using Rayn.Services.Responses;
using Rayn.ViewModels;

namespace Rayn.Controllers;

public class ThreadRoomController : Controller
{
    private readonly IThreadDbReader _threadDbReader;

    private readonly IMessageChannelStoreReader<ThreadMessage> _messageChannelStoreReader;

    private readonly IMessageChannelStoreCreator<ThreadMessage> _messageChannelStoreCreator;

    public ThreadRoomController(IThreadDbReader threadDbReader,
            IMessageChannelStoreReader<ThreadMessage> messageChannelStoreReader,
            IMessageChannelStoreCreator<ThreadMessage> messageChannelStoreCreator)
    {
        _threadDbReader = threadDbReader;
        _messageChannelStoreReader = messageChannelStoreReader;
        _messageChannelStoreCreator = messageChannelStoreCreator;
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

        var threadRoomViewModel = new ThreadRoomViewModel(thread.ThreadTitle, thread.ThreadId);

        return this.View(threadRoomViewModel);
    }

    [HttpGet]
    public async Task<ActionResult<StreamerConnectionResponse>> Streamer(string? threadId, string? ownerId, string? method)
    {
        if (threadId is null || ownerId is null || !Guid.TryParse(threadId, out var threadGuid) || !Guid.TryParse(ownerId, out var ownerGuid))
        {
            return new StreamerConnectionResponse(StreamerConnectionRequestStatus.BadRequest, string.Empty, Guid.Empty);
        }

        var thread = await _threadDbReader.SearchThreadModelAsync(threadGuid);

        if (thread is null)
        {
            return new StreamerConnectionResponse(StreamerConnectionRequestStatus.ThreadRoomNotExist, string.Empty, Guid.Empty);
        }

        if (ownerGuid != thread.OwnerId)
        {
            return new StreamerConnectionResponse(StreamerConnectionRequestStatus.BadRequest, string.Empty, Guid.Empty);
        }

        // SignalR使うのに、なんでpollingなんて自前実装しているかというと、Mac版のMonoでSignalR clientが動かないから。
        // Windowsだと動くけどMacOSだと動かないという...。
        if (!string.IsNullOrEmpty(method) && method == "polling")
        {
            var pollingUrl = this.PollingMessageUrl(threadGuid, ownerGuid);
            _messageChannelStoreCreator.Create(threadGuid);
            return new StreamerConnectionResponse(StreamerConnectionRequestStatus.Ok, pollingUrl, threadGuid);
        }

        var threadRoomHubUrl = this.ThreadRoomHubUrl();

        return new StreamerConnectionResponse(StreamerConnectionRequestStatus.Ok, threadRoomHubUrl, threadGuid);
    }

    private string ThreadRoomHubUrl()
    {
        var protocol = this.HttpContext.Request.Scheme;
        var host = this.HttpContext.Request.Host.Value;
        return ThreadRoomHub.Url(protocol, host);
    }

    private string PollingMessageUrl(Guid threadId, Guid ownerId)
    {
        var protocol = this.HttpContext.Request.Scheme;

        return this.Url.Action(
            nameof(this.FetchMessages),
            "ThreadRoom",
            new { threadId = threadId.ToString() },
            protocol)!;
    }

    [HttpGet]
    public IReadOnlyList<ThreadMessage> FetchMessages(Guid? threadId)
    {
        if (threadId is null)
        {
            return Array.Empty<ThreadMessage>();
        }

        var (isExist, messageChannel) = _messageChannelStoreReader.GetMessageChannel(threadId.Value);

        if (!isExist)
        {
            return Array.Empty<ThreadMessage>();
        }

        var messages = messageChannel!.ReadMessages();

        return messages;
    }

    [HttpGet]
    public IActionResult Error()
    {
        return this.View();
    }
}
