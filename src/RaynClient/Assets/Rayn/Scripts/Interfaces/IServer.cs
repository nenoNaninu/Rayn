using System.Threading;
using Cysharp.Threading.Tasks;

namespace Rayn
{
    public interface IServer<T>
    {
        UniTask<IMessageReceiver<string>> ConnectAsync(string url, string proxy ,CancellationToken cancellationToken = default);

        UniTask CloseAsync(CancellationToken cajCancellationToken);

        UniTask<bool> WaitUntilConnectAsync(CancellationToken cancellationToken = default);
        UniTask<IMessageReceiver<T>> GetMessageReceiverAsync(CancellationToken cancellationToken = default);
    }
}