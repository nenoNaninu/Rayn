using System.Threading.Tasks;
using RxWebSocket;
using ScreenOverwriterServer.Services.Database.Models;

namespace ScreenOverwriterServer.Services.Realtime
{
    public interface IThreadRoom
    {
        ThreadModel ThreadModel { get; }
        ValueTask AddAsync(IWebSocketClient newcomer);
    }
}