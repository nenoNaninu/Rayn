using System;

namespace ScreenOverwriter
{
    public interface INotificator
    {
        IObservable<IMessageReceiver<string>> OnSetMessageReceiver();
        void SetMessageReceiver(IMessageReceiver<string> receiver);
    }
}