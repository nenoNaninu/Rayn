using System.Threading;
using Cysharp.Threading.Tasks;

namespace ScreenOverwriter
{
    public interface IServer<T>
    {
        UniTask<IMessageReceiver<string>> ConnectAsync(string url, CancellationToken cancellationToken = default);
        UniTask CloseAsync(CancellationToken cancellationToken = default);
        UniTask WaitUntilConnectAsync(CancellationToken cancellationToken = default);
        UniTask<IMessageReceiver<T>> GetMessageReceiverAsync(CancellationToken cancellationToken = default);
    }
}