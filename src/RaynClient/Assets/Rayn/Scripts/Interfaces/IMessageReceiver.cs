using System;

namespace Rayn
{
    public interface IMessageReceiver<out T>
    {
        IObservable<T> OnMessage();
    }
}
