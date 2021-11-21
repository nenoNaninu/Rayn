using System;
using System.Collections.Generic;

namespace Rayn.Services.Realtime.Abstractions;

public interface IMessageChannel<T>
{
    void AddMessage(T message);
    IReadOnlyList<T> ReadMessages();
    DateTime LastUsed { get; }
}
