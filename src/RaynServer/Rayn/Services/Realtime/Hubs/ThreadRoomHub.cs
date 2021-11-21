using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Rayn.Services.Database.Abstractions;
using Rayn.Services.Realtime.Abstractions;
using Rayn.Services.Realtime.Hubs.Abstractions;
using Rayn.Services.Realtime.Models;

namespace Rayn.Services.Realtime.Hubs;

public class ThreadRoomHub : Hub<IThreadRoomClient>, IThreadRoomHub
{
    public const string Path = "/Realtime/ThreadRoom";

    private readonly IThreadDbReader _threadDbReader;
    private readonly ICommentAccessor _commentAccessor;
    private readonly IConnectionGroupCache _connectionGroupCache;
    private readonly IMessageChannelStoreReader<ThreadMessage> _messageChannelStoreReaderForPolling;

    public ThreadRoomHub(
        IConnectionGroupCache connectionGroupCache,
        IThreadDbReader threadDbReader,
        ICommentAccessor commentAccessor,
        IMessageChannelStoreReader<ThreadMessage> messageChannelStoreReaderForPolling)
    {
        _threadDbReader = threadDbReader;
        _commentAccessor = commentAccessor;
        _connectionGroupCache = connectionGroupCache;
        _messageChannelStoreReaderForPolling = messageChannelStoreReaderForPolling;
    }

    public async Task EnterThreadRoom(Guid threadId)
    {
        var thread = await _threadDbReader.SearchThreadModelAsync(threadId);

        if (thread is null)
        {
            await Clients.Caller.EnterRoomResultAsync(false, Array.Empty<ThreadMessage>());
        }

        string groupName = threadId.ToString();

        await Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);

        var group = new Group(groupName, threadId);
        _connectionGroupCache.Add(this.Context.ConnectionId, group);

        var comments = await _commentAccessor.ReadCommentAsync(group.ThreadId);

        var messages = comments.Select(x => new ThreadMessage { Message = x.Message }).ToArray();

        await Clients.Caller.EnterRoomResultAsync(true, messages);
    }

    public async Task PostMessageToServer(ThreadMessage message)
    {
        var group = _connectionGroupCache.FindGroup(this.Context.ConnectionId);

        if (group is null)
        {
            throw new Exception("Group is null");
        }

        await this.Clients.Group(group.GroupName).ReceiveMessageFromServer(message);

        var (isExist, messageChannel) = _messageChannelStoreReaderForPolling.GetMessageChannel(group.ThreadId);

        if (isExist)
        {
            messageChannel!.AddMessage(message);
        }

        await _commentAccessor.InsertCommentAsync(message.Message, group.ThreadId, DateTime.UtcNow);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _connectionGroupCache.Remove(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public static string Url(string protocol, string host) => $"{protocol}://{host}{Path}";
}
