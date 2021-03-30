using System.Threading.Tasks;
using Rayn.Services.Database.Models;

namespace Rayn.Services.Realtime
{
    public interface IThreadRoomCreator
    {
        ValueTask<IThreadRoom> CreateRoomAsync(ThreadModel threadModel);
    }
}