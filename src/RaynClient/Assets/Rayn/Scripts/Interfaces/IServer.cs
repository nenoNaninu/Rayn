using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Rayn
{
    public interface IServer<T> : IDisposable
    {
        UniTask CloseAsync(CancellationToken cancellationToken);
        UniTask<IMessageReceiver<string>> GenerateMessageReceiverAsync(string url, string proxy, CancellationToken cancellationToken);
        UniTask<IMessageReceiver<string>> GetMessageReceiverAsync(CancellationToken cancellationToken);
    }
}
