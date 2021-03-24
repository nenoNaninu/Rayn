using System;

namespace ScreenOverwriter
{
    public interface IMessageReceiver<out T>
    {
        IObservable<T> OnMessage();
    }
}