using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UniRx;

namespace ScreenOverwriter
{
    public class DummyServer : IServer
    {

        private class DummyReceiver : IMessageReceiver<string>
        {
            private readonly Subject<string> _subject = new Subject<string>();

            private readonly string[] _sampleTextArray = new string[]
            {
                "あ", "a", "メロンパン", "エヴァンゲリオン", "みんなシェルノサージュやろう!!!!!!😎", "😂", "😂😂😂😂😂😂😂😂😂", "Money is all you need", "なんとなくそれらしいのができた気がしないでもない。"
            };

            public IObservable<string> OnMessage() => _subject.AsObservable();

            public async UniTaskVoid Start()
            {
                await UniTask.Delay(TimeSpan.FromSeconds(5));

                foreach (string message in _sampleTextArray)
                {
                    _subject.OnNext(message);

                    await UniTask.Delay(TimeSpan.FromSeconds(1));
                }

                await UniTask.Delay(TimeSpan.FromSeconds(20));

                foreach (string message in _sampleTextArray)
                {
                    _subject.OnNext(message);

                    await UniTask.Delay(TimeSpan.FromSeconds(1));
                }
            }
        }

        private Subject<IMessageReceiver<string>> _subject;

        public DummyServer()
        {
        }

        public Task<IMessageReceiver<string>> ConnectAsync()
        {
            var receiver = new DummyReceiver();
            receiver.Start().Forget();
            return Task.FromResult(receiver as IMessageReceiver<string>);
        }

        public Task<bool> CloseAsync()
        {
            return Task.FromResult(true);
        }
    }
}