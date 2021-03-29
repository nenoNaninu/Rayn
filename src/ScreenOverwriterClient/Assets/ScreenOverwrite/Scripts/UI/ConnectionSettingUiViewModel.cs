using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using Cysharp.Threading.Tasks;
using Utf8Json;
using Utf8Json.Resolvers;

namespace ScreenOverwriter
{
    public class ConnectionSettingUiViewModel
    {
        private IServer<string> _server;

        public ConnectionSettingUiViewModel(IServer<string> server)
        {
            _server = server;
        }

        public async UniTask ConnectToServerAsync(string url, CancellationToken cancellationToken)
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

            await _server.ConnectAsync(content.RealtimeThreadRoomUrl, cancellationToken);
        }
    }
}