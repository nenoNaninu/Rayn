using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RxWebSocket;
using ScreenOverwriterServer.Services.Database.Interfaces;
using ScreenOverwriterServer.Services.Database.Models;
using ScreenOverwriterServer.Services.Threading;

namespace ScreenOverwriterServer.Services.Realtime
{
    public class ThreadRoom : IThreadRoom
    {
        public ThreadModel ThreadModel { get; }

        private readonly Channel<byte[]> _messageChannel = Channel.CreateUnbounded<byte[]>(
                new UnboundedChannelOptions()
                {
                    AllowSynchronousContinuations = false,
                    SingleReader = true,
                    SingleWriter = false
                });

        private readonly Subject<Unit> _onDisposeSubject = new();

        private readonly ChannelReader<byte[]> _channelReader;
        private readonly ChannelWriter<byte[]> _channelWriter;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly ICommentAccessor _commentAccessor;
        private readonly ILogger<IThreadRoom> _logger;
        private readonly Task _broadcastTask;

        private ImmutableList<IWebSocketClient> _webSocketClients = ImmutableList<IWebSocketClient>.Empty;
        private int _isStopRequested = 0;


        public ThreadRoom(ThreadModel threadModel, ICommentAccessor commentAccessor, ILogger<IThreadRoom> logger)
        {
            ThreadModel = threadModel;
            _commentAccessor = commentAccessor;
            _channelReader = _messageChannel.Reader;
            _channelWriter = _messageChannel.Writer;
            _logger = logger;
            _broadcastTask = Task.Run(this.StartBroadcastLoop);
        }


        private async Task StartBroadcastLoop()
        {
            try
            {
                while (_isStopRequested == 0 && await _channelReader.WaitToReadAsync(_cancellationTokenSource.Token).ConfigureAwait(false))
                {
                    while (_isStopRequested == 0 && _channelReader.TryRead(out byte[] message))
                    {
                        try
                        {
                            var clients = _webSocketClients.Data;

                            foreach (var client in clients)
                            {
                                client.Send(message);
                            }
                        }
                        catch (Exception e)
                        {
                            _logger?.LogError(e, $"Failed in broadcast message loop: '{message}'. Error: {e.Message}");
                        }
                    }
                }
            }
            catch (TaskCanceledException)
            {
                // task was canceled, ignore
            }
            catch (OperationCanceledException)
            {
                // operation was canceled, ignore
            }
            catch (Exception e)
            {
                if (_cancellationTokenSource.IsCancellationRequested)
                {
                    // disposing/canceling, do nothing and exit
                    return;
                }

                _logger?.LogError(e, $"Sending message thread failed, error: {e.Message}.");
            }
        }


        public async ValueTask AddAsync(IWebSocketClient newcomer)
        {
            // 新しい人が来たら、ソケットに対していくつかの設定を行う。
            // 1.新しい人に今までのスレッドを一機に放出。
            // 2.メッセージをソケットが受信した時の設定。
            // 3.ソケットが切断した時の設定

            var pastComments = await _commentAccessor.ReadCommentAsync(this.ThreadModel.ThreadId);

            foreach (var comment in pastComments)
            {
                newcomer.Send(comment.Message);
            }

            _webSocketClients = _webSocketClients.Add(newcomer);

            newcomer.BinaryMessageReceived
                .Subscribe(x =>
                {
                    var timeStamp = DateTime.UtcNow;
                    this.BroadCast(x);
                    _commentAccessor.InsertCommentAsync(x, this.ThreadModel.ThreadId, timeStamp).Forget(_logger);
                });

            newcomer.CloseMessageReceived
                .Subscribe(x =>
                {
                    this.OnClose(newcomer);
                });
        }

        public IObservable<Unit> OnDispose()
        {
            return _onDisposeSubject.AsObservable();
        }

        private void OnClose(IWebSocketClient client)
        {
            _webSocketClients = _webSocketClients.Remove(client);

            if (_webSocketClients == ImmutableList<IWebSocketClient>.Empty)
            {
                this.Dispose();
            }
        }

        public void BroadCast(byte[] data)
        {
            _channelWriter.TryWrite(data);
        }

        public void Dispose()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            Interlocked.Increment(ref _isStopRequested);
            _cancellationTokenSource.Cancel();
            _onDisposeSubject.OnNext(Unit.Default);
            _onDisposeSubject.Dispose();
        }
    }
}