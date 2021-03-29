﻿using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using RxWebSocket;
using RxWebSocket.Logging;
using RxWebSocket.Senders;
using UniRx;
using Utf8Json;

namespace ScreenOverwriter
{
    public class Server : IServer<string>
    {
        private readonly UniTaskCompletionSource _getReceiverCompletionSource = new UniTaskCompletionSource();
        private readonly UniTaskCompletionSource<bool> _waitConnectionCompletionSource = new UniTaskCompletionSource<bool>();

        private IMessageReceiver<string> _messageReceiver;

        public async UniTask<IMessageReceiver<string>> ConnectAsync(string url, CancellationToken cancellationToken = default)
        {
            try
            {
                var socket = new WebSocketClient(new Uri(url), new BinaryOnlySender(), logger: new UnityConsoleLogger());
                await socket.ConnectAsync();
                _waitConnectionCompletionSource.TrySetResult(true);
                _messageReceiver = new MessageReceiver(socket);

                _getReceiverCompletionSource.TrySetResult();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }

            return _messageReceiver;
        }

        public async UniTask<bool> WaitUntilConnectAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _waitConnectionCompletionSource.Task;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public async UniTask<IMessageReceiver<string>> GetMessageReceiverAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _getReceiverCompletionSource.Task;
                return _messageReceiver;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private class MessageReceiver : IMessageReceiver<string>
        {
            private readonly WebSocketClient _client;

            public MessageReceiver(WebSocketClient socket)
            {
                _client = socket;
            }

            public IObservable<string> OnMessage()
            {
                return _client.BinaryMessageReceived
                    //.Do(x => Debug.WriteLine($"Receive data length: {x.Length}"))
                    .Select(x => JsonSerializer.Deserialize<ReceiveData>(x))
                    //.Do(x => Debug.WriteLine($"Receive :{x.Message}"))
                    .Select(x => x.Message);
            }
        }
    }
}