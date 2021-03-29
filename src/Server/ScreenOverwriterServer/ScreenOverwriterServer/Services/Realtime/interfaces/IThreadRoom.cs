using System;
using System.Reactive;
using System.Threading.Tasks;
using RxWebSocket;
using ScreenOverwriterServer.Services.Database.Models;

namespace ScreenOverwriterServer.Services.Realtime
{
    public interface IThreadRoom : IDisposable
    {
        ThreadModel ThreadModel { get; }
        ValueTask AddAsync(IWebSocketClient newcomer);

        IObservable<Unit> OnDispose();
    }
}