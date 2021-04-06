using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Rayn
{
    public interface IServer<T> : IDisposable
    {
        //UniTask<IMessageReceiver<string>> ConnectAsync(string url, string proxy ,CancellationToken cancellationToken = default);

        UniTask CloseAsync(CancellationToken cancellationToken);

        //UniTask<bool> WaitUntilConnectAsync(CancellationToken cancellationToken = default);

        //UniTask<IMessageReceiver<T>> GetMessageReceiverAsync(CancellationToken cancellationToken = default);

        UniTask<IMessageReceiver<string>> GenerateMessageReceiverAsync(string url, string proxy, CancellationToken cancellationToken);

        UniTask<IMessageReceiver<string>> GetMessageReceiverAsync(CancellationToken cancellationToken);
    }
}