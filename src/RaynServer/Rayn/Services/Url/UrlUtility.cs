using System;
using Rayn.Controllers;
using Rayn.Services.Utilities;

namespace Rayn.Services.Url;

public static class UrlUtility
{
    public static string ThreadUrl(string hostDomain, Guid threadId)
        => $"https://{hostDomain}/{ControllerRoute.Value<ThreadRoomController>()}?threadId={threadId.ToString()}";

    public static string StreamerUrl(string hostDomain, Guid threadId, Guid ownerId)
        => $"https://{hostDomain}/{ControllerRoute.Value<ThreadRoomController>()}/{nameof(ThreadRoomController.Streamer)}?threadId={threadId.ToString()}&ownerId={ownerId.ToString()}";
}
