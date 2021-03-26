using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RxWebSocket;
using ScreenOverwriterServer.Services.Database.Models;
using ScreenOverwriterServer.Services.Threading;

namespace ScreenOverwriterServer.Services.Realtime
{
    public class ThreadRoom : IThreadRoom
    {
        public ThreadModel ThreadModel { get; }

        private readonly Channel<string> _messageChannel = Channel.CreateUnbounded<string>(
                new UnboundedChannelOptions()
                {
                    AllowSynchronousContinuations = false,
                    SingleReader = true,
                    SingleWriter = false
                });

        private readonly ChannelReader<string> _channelReader;
        private readonly ChannelWriter<string> _channelWriter;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly ILogger<IThreadRoom> _logger;
        private readonly Task _broadcastTask;
        
        private ImmutableList<IWebSocketClient> _webSocketClients = ImmutableList<IWebSocketClient>.Empty;
        private int _isStopRequested = 0;


        public ThreadRoom(ThreadModel threadModel, ILogger<IThreadRoom> logger)
        {
            ThreadModel = threadModel;
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
                    while (_isStopRequested == 0 && _channelReader.TryRead(out var message))
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


        public ValueTask Add(IWebSocketClient newcomer)
        {
            _webSocketClients = _webSocketClients.Add(newcomer);
            return ValueTask.CompletedTask;
        }

        private void OnClose(WebSocketClient client)
        {
            _webSocketClients = _webSocketClients.Remove(client);
        }

        private void Destory()
        {

        }

        public void BroadCast()
        {

        }

    }
}