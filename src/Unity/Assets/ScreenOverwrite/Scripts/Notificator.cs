using System;
using UniRx;

namespace ScreenOverwriter
{
    public class Notificator : INotificator
    {
        private readonly Subject<IMessageReceiver<string>> _receiverSubject = new Subject<IMessageReceiver<string>>();

        public IObservable<IMessageReceiver<string>> OnSetMessageReceiver()
        {
            return _receiverSubject.AsObservable();
        }

        public void SetMessageReceiver(IMessageReceiver<string> receiver)
        {
            _receiverSubject.OnNext(receiver);
        }
    }
}