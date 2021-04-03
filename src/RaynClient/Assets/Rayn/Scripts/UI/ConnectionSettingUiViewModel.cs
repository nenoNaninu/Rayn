using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Rayn
{
    public class ConnectionSettingUiViewModel
    {
        private readonly IServer<string> _server;

        public ConnectionSettingUiViewModel(IServer<string> server)
        {
            _server = server;
        }

        public async UniTask ConnectToServerAsync(string url, string proxy, CancellationToken cancellationToken)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url, cancellationToken);

            var contentBytes = await response.Content.ReadAsByteArrayAsync();

            var content = JsonSerializer.Deserialize<StreamerConnectionResponse>(contentBytes, StandardResolver.CamelCase);

            if (content.RequestStatus != StreamerConnectionRequestStatus.Ok)
            {
                Debug.WriteLine(content.RealtimeThreadRoomUrl);
                Debug.WriteLine(content.RequestStatus);
            }

            await _server.ConnectAsync(content.RealtimeThreadRoomUrl, proxy, cancellationToken);
        }

        public async UniTask CloseAsync(CancellationToken cancellationToken)
        {
            await _server.CloseAsync(cancellationToken);
        }
    }
}