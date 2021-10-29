using System.Threading;
using Cysharp.Threading.Tasks;

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
            await _server.GenerateMessageReceiverAsync(url, proxy, cancellationToken);
        }

        public async UniTask CloseAsync(CancellationToken cancellationToken)
        {
            await _server.CloseAsync(cancellationToken);
            _server.Dispose();
        }
    }
}
