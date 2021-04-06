using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using Rayn.Services.Realtime.Interfaces;
using Rayn.Services.Realtime.Models;

namespace Rayn.Services.Realtime
{
    public sealed class HttpPollingUserConnection : IPollingUserConnection
    {
        private readonly Channel<string> _messageChannel = Channel.CreateUnbounded<string>(
            new UnboundedChannelOptions()
            {
                AllowSynchronousContinuations = false,
                SingleReader = true,
                SingleWriter = false,
            });

        private readonly ChannelReader<string> _messageChannelReader;
        private readonly ChannelWriter<string> _messageChannelWriter;

        private readonly object _lockObj = new();

        private readonly Subject<Unit> _disposeSubject = new();

        private readonly Timer _timer;

        private DateTime _latestReadTime = DateTime.Now;

        public HttpPollingUserConnection()
        {
            _messageChannelReader = _messageChannel.Reader;
            _messageChannelWriter = _messageChannel.Writer;

            // しばらく接続されなかったら自滅してもらう。
            _timer = new Timer(_ =>
            {
                if (_latestReadTime + TimeSpan.FromMinutes(10) < DateTime.UtcNow)
                {
                    this.Dispose();
                }
            }, null, TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2));
        }

        public IReadOnlyList<string> ReadSentMessages()
        {
            // 原則ここが複数スレッドから同時に呼ばれることはないはず。(複数の配信クライアントで同じURLでポーリングすると発生するけど、普通の用途じゃない)
            // 念のためロックするけど、原則同時に呼ばれないのでAsyncLockじゃなくて素朴にlockで十分と判断。
            lock (_lockObj)
            {
                _latestReadTime = DateTime.UtcNow;
                var list = new List<string>();

                // 1秒以内にリクエストは返す。
                var timeLimit = DateTime.UtcNow + TimeSpan.FromSeconds(1);

                while (DateTime.UtcNow < timeLimit && _messageChannelReader.TryRead(out string message))
                {
                    list.Add(message);
                }

                return list;
            }
        }

        public IObservable<Unit> OnDispose()
        {
            return _disposeSubject.AsObservable();
        }

        public void Dispose()
        {
            try
            {
                _messageChannel.Writer.TryComplete();
                _disposeSubject.OnNext(Unit.Default);
                _disposeSubject.Dispose();
            }
            catch
            {
                // ignore
            }
        }

        public void Send(byte[] data)
        {
            try
            {
                string message = JsonSerializer.Deserialize<MessageModel>(data)?.Message;

                if (string.IsNullOrEmpty(message))
                {
                    return;
                }

                _messageChannelWriter.TryWrite(message);
            }
            catch
            {
                // ignored
            }
        }
    }
}