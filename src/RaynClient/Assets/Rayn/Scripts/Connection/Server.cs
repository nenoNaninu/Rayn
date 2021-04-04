using System;
using System.Diagnostics;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using Cysharp.Threading.Tasks;
using RxWebSocket;
using RxWebSocket.Logging;
using RxWebSocket.Senders;
using UniRx;
using Utf8Json;

namespace Rayn
{
    public class Server : IServer<string>
    {
        private readonly UniTaskCompletionSource _getReceiverCompletionSource = new UniTaskCompletionSource();
        private readonly UniTaskCompletionSource<bool> _waitConnectionCompletionSource = new UniTaskCompletionSource<bool>();
        private WebSocketClient _socket;

        private IMessageReceiver<string> _messageReceiver;

        public async UniTask<IMessageReceiver<string>> ConnectAsync(string url, string proxy, CancellationToken cancellationToken = default)
        {
            try
            {
                var clientFactory = CreateClientFactory(proxy);

                _socket = new WebSocketClient(new Uri(url), new BinaryOnlySender(), logger: new UnityConsoleLogger(), clientFactory: clientFactory);
                await _socket.ConnectAsync();
                _waitConnectionCompletionSource.TrySetResult(true);
                _messageReceiver = new MessageReceiver(_socket);

                _getReceiverCompletionSource.TrySetResult();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }

            return _messageReceiver;
        }

        public async UniTask CloseAsync(CancellationToken cajCancellationToken)
        {
            if (_socket == null || _socket.IsClosed)
            {
                return;
            }

            try
            {
                await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Click close button");
            }
            catch
            {
                return;
            }
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

        private static Func<ClientWebSocket> CreateClientFactory(string proxy)
        {
            return null;
            if (string.IsNullOrEmpty(proxy))
            {
                return null;
            }

            return () => new ClientWebSocket
            {
                Options =
                {
                    KeepAliveInterval = TimeSpan.FromSeconds(5),
                    Proxy = new WebProxy(new Uri(proxy),true)
                }
            };
        }
    }
}