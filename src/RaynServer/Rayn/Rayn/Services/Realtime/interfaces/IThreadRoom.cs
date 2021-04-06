using System;
using System.Reactive;
using System.Threading.Tasks;
using Rayn.Services.Database.Models;
using RxWebSocket;

namespace Rayn.Services.Realtime.Interfaces
{
    public interface IThreadRoom : IDisposable
    {
        ThreadModel ThreadModel { get; }
        ValueTask<bool> AddAsync(IWebSocketClient newcomer);
        IObservable<Unit> OnDispose();
    }
}