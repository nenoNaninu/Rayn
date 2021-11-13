using System;
using System.Collections.Concurrent;
using System.Threading;
using Rayn.Services.Realtime.Interfaces;

namespace Rayn.Services.Realtime;

public class MessageChannelStore<T> : IMessageChannelStoreReader<T>, IMessageChannelStoreCreator<T>
{
    private readonly ConcurrentDictionary<Guid, IMessageChannel<T>> _messageChannelDictionary = new();
    private readonly Timer _timer;

    public MessageChannelStore()
    {
        _timer = new Timer(_ =>
        {
            foreach (var keyValuePair in _messageChannelDictionary)
            {
                if (keyValuePair.Value.LastUsed + TimeSpan.FromMinutes(10) < DateTime.UtcNow)
                {
                    _messageChannelDictionary.TryRemove(keyValuePair);
                }
            }
        }, null, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10));
    }

    public (bool isExist, IMessageChannel<T>? messageChannel) GetMessageChannel(Guid threadId)
    {
        var isExist = _messageChannelDictionary.TryGetValue(threadId, out var messageChannel);
        return (isExist, messageChannel);
    }

    public void Create(Guid threadId)
    {
        var thread = new MessageChannel<T>();
        _messageChannelDictionary.TryAdd(threadId, thread);
    }
}
