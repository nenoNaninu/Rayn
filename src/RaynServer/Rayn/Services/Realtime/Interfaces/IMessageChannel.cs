using System;
using System.Collections.Generic;

namespace Rayn.Services.Realtime.Interfaces
{
    public interface IMessageChannel<T>
    {
        void AddMessage(T message);
        IReadOnlyList<T> ReadMessages();
        DateTime LastUsed { get; }
    }
}