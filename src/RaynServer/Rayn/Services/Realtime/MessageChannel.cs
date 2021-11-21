using System;
using System.Collections.Generic;
using System.Threading.Channels;
using Rayn.Services.Realtime.Abstractions;

namespace Rayn.Services.Realtime;

public sealed class MessageChannel<T> : IMessageChannel<T>
{
    private readonly Channel<T> _channel = Channel.CreateUnbounded<T>(new UnboundedChannelOptions()
    {
        AllowSynchronousContinuations = false,
        SingleReader = true,
        SingleWriter = false,
    });

    private readonly object _readLocker = new();
    private readonly ChannelReader<T> _channelReader;
    private readonly ChannelWriter<T> _channelWriter;
    private readonly List<T> _buffer = new(32);

    public MessageChannel()
    {
        _channelReader = _channel.Reader;
        _channelWriter = _channel.Writer;
    }

    public DateTime LastUsed { get; private set; }

    public void AddMessage(T message)
    {
        LastUsed = DateTime.UtcNow;
        _channelWriter.TryWrite(message);
    }

    public IReadOnlyList<T> ReadMessages()
    {
        lock (_readLocker)
        {
            LastUsed = DateTime.UtcNow;

            // 1秒以内にリクエストは返す。
            var timeLimit = DateTime.UtcNow + TimeSpan.FromSeconds(1);

            _buffer.Clear();

            while (DateTime.UtcNow < timeLimit && _channelReader.TryRead(out var message))
            {
                _buffer.Add(message);
            }

            return _buffer.Count == 0 ? Array.Empty<T>() : _buffer.ToArray();
        }
    }
}
