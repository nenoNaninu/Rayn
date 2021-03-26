using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ScreenOverwriterServer.Services.Database.Interfaces;
using ScreenOverwriterServer.Services.Database.Models;

namespace ScreenOverwriterServer.Services.Realtime
{
    public interface IThreadRoomCreator
    {
        ValueTask<IThreadRoom> CreateRoomAsync(ThreadModel threadModel);
    }
}