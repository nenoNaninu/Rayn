using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;

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
            await _server.ConnectAsync(url, cancellationToken);
        }
    }
}