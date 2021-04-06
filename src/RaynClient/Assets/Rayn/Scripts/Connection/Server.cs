using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading;
using Cysharp.Threading.Tasks;
using RxWebSocket;
using RxWebSocket.Logging;
using RxWebSocket.Senders;
using UniRx;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Rayn
{
    public sealed class Server : IServer<string>
    {
        private readonly UniTaskCompletionSource<bool> _waitConnectionCompletionSource = new UniTaskCompletionSource<bool>();

        private WebSocketClient _socket;
        private HttpClient _httpClient;

        private IMessageReceiver<string> _messageReceiver;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();


        private void Init(string proxy)
        {
            _httpClient = string.IsNullOrEmpty(proxy)
                ? new HttpClient()
                : new HttpClient(new HttpClientHandler() { UseProxy = true, Proxy = new WebProxy(proxy) });
        }

        public async UniTask<IMessageReceiver<string>> GenerateMessageReceiverAsync(string url, string proxy, CancellationToken cancellationToken)
        {
            if (_httpClient == null)
            {
                this.Init(proxy);
            }

            if (!string.IsNullOrEmpty(proxy))
            {
                url += "&method=polling";
            }

            var response = await _httpClient.GetAsync(url, cancellationToken);

            var contentBytes = await response.Content.ReadAsByteArrayAsync();

            var content = JsonSerializer.Deserialize<StreamerConnectionResponse>(contentBytes, StandardResolver.CamelCase);

            if (string.IsNullOrEmpty(proxy))
            {
                // websocket
                _messageReceiver = await this.ConnectWebSocketMessageReceiver(content.RealtimeThreadRoomUrl, cancellationToken);
                await UniTask.SwitchToMainThread(PlayerLoopTiming.Update);
            }
            else
            {
                // http polling
                // Monoのバグで、websocketのproxyがまともに動作しないので仕方なくpollingで誤魔化す。
                _messageReceiver = new PollingMessageReceiver(this, content.RealtimeThreadRoomUrl, _cancellationTokenSource.Token);
                await UniTask.SwitchToMainThread(PlayerLoopTiming.Update);
            }

            return _messageReceiver;
        }

        public async UniTask<IMessageReceiver<string>> GetMessageReceiverAsync(CancellationToken cancellationToken = default)
        {
            await UniTask.WaitUntil(() => _messageReceiver != null, cancellationToken: cancellationToken);
            return _messageReceiver;
        }

        private async UniTask<IMessageReceiver<string>> ConnectWebSocketMessageReceiver(string url, CancellationToken cancellationToken)
        {
            try
            {
                _socket = new WebSocketClient(new Uri(url), new BinaryOnlySender(), logger: new UnityConsoleLogger());
                // ConnectAsyncにcancellationToken投げられるように更新しないとアカン...(内部のcancellationToken使うようにして、Dispose叩かれた時に全部死ぬようにしてる)
                await _socket.ConnectAsync();
                _waitConnectionCompletionSource.TrySetResult(true);
                _messageReceiver = new WebSocketMessageReceiver(_socket);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }

            return _messageReceiver;
        }

        public async UniTask CloseAsync(CancellationToken cancellationToken)
        {
            if (_socket != null && _socket.IsClosed)
            {
                return;
            }

            try
            {
                if (_socket != null)
                {
                    await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Click close button");
                }

                _cancellationTokenSource.Cancel();
            }
            catch
            {
                return;
            }
        }

        private class WebSocketMessageReceiver : IMessageReceiver<string>
        {
            private readonly WebSocketClient _client;

            public WebSocketMessageReceiver(WebSocketClient socket)
            {
                _client = socket;
            }

            public IObservable<string> OnMessage()
            {
                return _client.BinaryMessageReceived
                    .Select(x => JsonSerializer.Deserialize<ReceiveData>(x))
                    .Select(x => x.Message);
            }
        }

        private class PollingMessageReceiver : IMessageReceiver<string>
        {
            private readonly Server _parent;
            private readonly HttpClient _httpClient;

            private readonly Subject<string> _onMessageSubject = new Subject<string>();

            private readonly Channel<string> _channel = Channel.CreateSingleConsumerUnbounded<string>();
            private readonly ChannelWriter<string> _channelWriter;
            private readonly ChannelReader<string> _channelReader;

            public PollingMessageReceiver(Server parent, string url, CancellationToken cancellationToken)
            {
                _parent = parent;
                _httpClient = parent._httpClient;

                _channelReader = _channel.Reader;
                _channelWriter = _channel.Writer;

                this.Polling(url, cancellationToken).Forget();
            }

            /// <summary>
            /// 5秒間毎にリクエスト飛ばす。
            /// </summary>
            private async UniTaskVoid Polling(string url, CancellationToken cancellationToken)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(10), cancellationToken: cancellationToken);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var response = await _httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);

                        byte[] contentBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                        string[] messages = JsonSerializer.Deserialize<string[]>(contentBytes, StandardResolver.CamelCase);

                        foreach (string message in messages)
                        {
                            _channelWriter.TryWrite(message);
                        }

                        await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: cancellationToken);
                    }
                }
                catch
                {
                    // ignore
                }
                finally
                {
                    _onMessageSubject.Dispose();
                }
            }

            public IObservable<string> OnMessage()
            {
                return Observable.Interval(TimeSpan.FromMilliseconds(300))
                    .Select(_ => _channelReader.TryRead(out string item) ? item : null)
                    .Where(x => x != null);
            }
        }

        public void Dispose()
        {
            _socket?.Dispose();
            _httpClient?.Dispose();
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}