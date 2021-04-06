﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Database.Models;
using Rayn.Services.Realtime.Interfaces;
using Rayn.Services.Realtime.Models;
using Rayn.Services.Threading;
using RxWebSocket;

namespace Rayn.Services.Realtime
{
    public sealed class ThreadRoom : IThreadRoom
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

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private readonly ICommentAccessor _commentAccessor;
        private readonly ILogger<IThreadRoom> _logger;
        private readonly Task _broadcastTask;
        private readonly object _entryAndExitLock = new();

        private ImmutableList<IUserConnection> _userConnections = ImmutableList<IUserConnection>.Empty;
        private int _isDisposed = 0;


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
                while (_isDisposed == 0 && await _channelReader.WaitToReadAsync(_cancellationTokenSource.Token).ConfigureAwait(false))
                {
                    while (_isDisposed == 0 && _channelReader.TryRead(out byte[] message))
                    {
                        try
                        {
                            var clients = _userConnections.Data;

                            // ここがあるからImmutableList使ってる。
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

        public static byte[] PingPongBytes { get; } = JsonSerializer.SerializeToUtf8Bytes(new MessageModel(null, true));

        public async ValueTask<bool> AddAsync(IWebSocketClient newcomer)
        {
            // 新しい人が来たら、ソケットに対していくつかの設定を行う。
            // 1.新しい人に今までのスレッドを一機に放出。
            // 2.メッセージをソケットが受信した時の設定。
            // 3.ソケットが切断した時の設定

            var pastComments = await _commentAccessor.ReadCommentAsync(this.ThreadModel.ThreadId);

            IUserConnection newComerConnection = new WebSocketUserConnection(newcomer);

            foreach (var comment in pastComments)
            {
                var model = new MessageModel(comment.Message, false);
                newcomer.Send(JsonSerializer.SerializeToUtf8Bytes(model));
            }

            lock (_entryAndExitLock)
            {
                if (_isDisposed != 0)
                {
                    return false;
                }

                _userConnections = _userConnections.Add(newComerConnection);
            }

            newcomer.BinaryMessageReceived
                .Subscribe(x =>
                {
                    try
                    {
                        var model = JsonSerializer.Deserialize<MessageModel>(x);

                        if (model == null)
                        {
                            return;
                        }

                        if (model.PingPong)
                        {
                            newcomer.Send(PingPongBytes);
                        }
                        else
                        {
                            var timeStamp = DateTime.UtcNow;
                            this.BroadCast(x);
                            _commentAccessor.InsertCommentAsync(model.Message, this.ThreadModel.ThreadId, timeStamp).Forget(_logger);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, $"[BinaryMessageReceived.Subscribe] {e.Message}");
                    }
                });

            newcomer.OnDispose
                .Subscribe(x =>
                {
                    this.OnDisposeWebSocketClient(newComerConnection);
                });

            return true;
        }

        public IObservable<Unit> OnDispose()
        {
            return _onDisposeSubject.AsObservable();
        }

        private void OnDisposeWebSocketClient(IUserConnection client)
        {
            lock (_entryAndExitLock)
            {
                _userConnections = _userConnections.Remove(client);

                if (_userConnections == ImmutableList<IUserConnection>.Empty)
                {
                    this.DisposeCore();
                }
            }
        }

        private void BroadCast(byte[] data)
        {
            _channelWriter.TryWrite(data);
        }

        private void DisposeCore()
        {
            if (Interlocked.Increment(ref _isDisposed) == 1)
            {
                _cancellationTokenSource.Cancel();
                _onDisposeSubject.OnNext(Unit.Default);
                _onDisposeSubject.OnCompleted();
                _onDisposeSubject.Dispose();

                var sockets = _userConnections.Data;

                foreach (var socket in sockets)
                {
                    socket.Dispose();
                }

                _userConnections = ImmutableList<IUserConnection>.Empty;
            }
        }

        public void Dispose()
        {
            if (_isDisposed != 0)
            {
                return;
            }

            lock (_entryAndExitLock)
            {
                this.DisposeCore();
            }
        }
    }
}