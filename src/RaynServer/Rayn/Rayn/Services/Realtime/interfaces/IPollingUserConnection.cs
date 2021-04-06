using System;
using System.Collections.Generic;
using System.Reactive;

namespace Rayn.Services.Realtime.Interfaces
{
    public interface IPollingUserConnection : IUserConnection
    {
        IReadOnlyList<string> ReadSentMessages();
        IObservable<Unit> OnDispose();
    }
}