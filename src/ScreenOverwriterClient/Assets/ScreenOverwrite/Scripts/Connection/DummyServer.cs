using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace ScreenOverwriter
{
    public class DummyServer : IServer<string>
    {
        private readonly UniTaskCompletionSource _getSocketCompletionSource = new UniTaskCompletionSource();
        private readonly UniTaskCompletionSource _waitConnectionCompletionSource = new UniTaskCompletionSource();

        private IMessageReceiver<string> _messageReceiver;

        private class DummyReceiver : IMessageReceiver<string>
        {
            private readonly Subject<string> _onMessageSubject = new Subject<string>();

            private readonly string[] _sampleTextArray = new string[]
            {
                "あ", "a", "メロンパン", "エヴァンゲリオン", "みんなシェルノサージュやろう!!!!!!😎", "😂", "😂😂😂😂😂😂😂😂😂", "Money is all you need", "なんとなくそれらしいのができた気がしないでもない。"
            };

            public IObservable<string> OnMessage() => _onMessageSubject.AsObservable();

            public async UniTaskVoid Start()
            {
                await UniTask.Delay(TimeSpan.FromSeconds(5));

                foreach (string message in _sampleTextArray)
                {
                    _onMessageSubject.OnNext(message);

                    await UniTask.Delay(TimeSpan.FromSeconds(1));
                }

                await UniTask.Delay(TimeSpan.FromSeconds(20));

                foreach (string message in _sampleTextArray)
                {
                    _onMessageSubject.OnNext(message);

                    await UniTask.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }

        private Subject<IMessageReceiver<string>> _subject;

        public DummyServer()
        {
        }

        public UniTask CloseAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public UniTask WaitUntilConnectAsync(CancellationToken cancellationToken = default)
        {
            return _waitConnectionCompletionSource.Task;
        }

        public async UniTask<IMessageReceiver<string>> ConnectAsync(string url, CancellationToken cancellationToken = default)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: cancellationToken);

            var receiver = new DummyReceiver();
            receiver.Start().Forget();
            _messageReceiver = receiver;
            
            _getSocketCompletionSource.TrySetResult();
            _waitConnectionCompletionSource.TrySetResult();

            return receiver;
        }

        public async UniTask<IMessageReceiver<string>> GetMessageReceiverAsync(CancellationToken cancellationToken = default)
        {
            await _getSocketCompletionSource.Task.AttachExternalCancellation(cancellationToken);
            return _messageReceiver;
        }
    }
}