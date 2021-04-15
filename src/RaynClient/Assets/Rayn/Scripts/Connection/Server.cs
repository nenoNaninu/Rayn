using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Cysharp.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using UniRx;
using Utf8Json;
using Utf8Json.Resolvers;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

namespace Rayn
{
    public sealed class Server : IServer<string>
    {
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

            if (content.RequestStatus != StreamerConnectionRequestStatus.Ok)
            {
                throw new Exception(content.RequestStatus.ToString());
            }

            if (string.IsNullOrEmpty(proxy))
            {
                // websocket
                _messageReceiver = await this.ConnectSignalRConnection(content.RealtimeThreadRoomUrl, content.ThreadId, cancellationToken);
                await UniTask.SwitchToMainThread(PlayerLoopTiming.Update);
            }
            else
            {
                _messageReceiver = await this.ConnectSignalRConnectionWithProxy(content.RealtimeThreadRoomUrl, proxy, content.ThreadId, _cancellationTokenSource.Token);
                await UniTask.SwitchToMainThread(PlayerLoopTiming.Update);
            }

            return _messageReceiver;
        }

        public async UniTask<IMessageReceiver<string>> GetMessageReceiverAsync(CancellationToken cancellationToken = default)
        {
            await UniTask.WaitUntil(() => _messageReceiver != null, cancellationToken: cancellationToken);
            return _messageReceiver;
        }

        private async UniTask<IMessageReceiver<string>> ConnectSignalRConnection(string url, Guid threadId, CancellationToken cancellationToken)
        {
            try
            {
                var messageReceiver = new SignalRMessageReceiver(url, threadId);
                await messageReceiver.StartAsync(cancellationToken);
                await messageReceiver.WaitEnterRoomAsync();
                _messageReceiver = messageReceiver;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }

            return _messageReceiver;
        }

        private async UniTask<IMessageReceiver<string>> ConnectSignalRConnectionWithProxy(string url, string proxy, Guid threadId, CancellationToken cancellationToken)
        {
            try
            {
                var messageReceiver = new SignalRMessageReceiver(url, proxy, threadId);
                await messageReceiver.StartAsync(cancellationToken);
                await messageReceiver.WaitEnterRoomAsync();
                _messageReceiver = messageReceiver;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }

            return _messageReceiver;
        }

        public async UniTask CloseAsync(CancellationToken cancellationToken)
        {
            if (_messageReceiver is SignalRMessageReceiver messageReceiver)
            {
                await messageReceiver.StopAsync(cancellationToken);
            }
        }

        public void Dispose()
        {
            this.CloseAsync(CancellationToken.None).Forget();
        }

        private class SignalRMessageReceiver : IMessageReceiver<string>
        {
            private readonly HubConnection _connection;
            private readonly Subject<string> _messageSubject = new Subject<string>();

            private readonly UniTaskCompletionSource _wait = new UniTaskCompletionSource();

            public UniTask WaitEnterRoomAsync() => _wait.Task;

            private readonly Guid _threadId;

            public SignalRMessageReceiver(string url, Guid threadId)
            {
                HubConnection connection = new HubConnectionBuilder()
                    .WithUrl(url)
                    .WithAutomaticReconnect()
                    .Build();
                

                _connection = connection;
                _threadId = threadId;

                this.InitReceiveSettings();
            }

            public SignalRMessageReceiver(string url, string proxy, Guid threadId)
            {
                HubConnection connection = new HubConnectionBuilder()
                    .WithUrl(url, option =>
                    {
                        option.Proxy = new WebProxy(proxy);
                        // Monoのwebsocket実装はproxyがバグっているので除外する。
                        option.Transports = HttpTransportType.ServerSentEvents | HttpTransportType.LongPolling;
                    })
                    .WithAutomaticReconnect()
                    .Build();


                _connection = connection;
                _threadId = threadId;

                this.InitReceiveSettings();
            }

            private void InitReceiveSettings()
            {
                _connection.On<bool, ThreadMessage[]>("EnterRoomResultAsync", (result, messages) =>
                {
                    if (result)
                    {
                        _wait.TrySetResult();
                    }
                    else
                    {
                        _wait.TrySetException(new Exception("Cannot enter thread room!"));
                    }
                });

                _connection.On<ThreadMessage>("ReceiveMessageFromServer", message =>
                {
                    if (!string.IsNullOrEmpty(message?.Message))
                    {
                        _messageSubject.OnNext(message.Message);
                    }
                });
            }

            public async UniTask StartAsync(CancellationToken cancellationToken)
            {
                await _connection.StartAsync(cancellationToken).ConfigureAwait(false);

                await _connection.InvokeAsync("EnterThreadRoom", _threadId, cancellationToken).ConfigureAwait(false);
            }

            public async UniTask StopAsync(CancellationToken cancellationToken)
            {
                await _connection.StopAsync(cancellationToken).ConfigureAwait(false);
            }

            public IObservable<string> OnMessage()
            {
                return _messageSubject.AsObservable();
            }
        }
    }
}