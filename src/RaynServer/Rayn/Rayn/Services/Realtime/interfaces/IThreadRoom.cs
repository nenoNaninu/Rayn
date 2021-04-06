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

        // AddAsync(IUserConnection newcomer)みたいな感じで統一したほうがキレイには感じるが...。ルームの中で設定いろいろしたいので悩ましい。
        ValueTask<bool> AddAsync(IWebSocketClient newcomer);
        ValueTask<bool> AddAsync(IPollingUserConnection newcomer);
        IObservable<Unit> OnDispose();
    }
}