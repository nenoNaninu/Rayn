using System;
using System.Threading.Tasks;

namespace Rayn.Services.Realtime.Interfaces
{
    public interface IThreadRoomStore
    {
        ValueTask<IThreadRoom> FetchThreadRoomAsync(Guid threadId);
    }
}