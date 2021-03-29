using System.Threading.Tasks;
using ScreenOverwriterServer.Services.Database.Models;

namespace ScreenOverwriterServer.Services.Realtime
{
    public interface IThreadRoomCreator
    {
        ValueTask<IThreadRoom> CreateRoomAsync(ThreadModel threadModel);
    }
}