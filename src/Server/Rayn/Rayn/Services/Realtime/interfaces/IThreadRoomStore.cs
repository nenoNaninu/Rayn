using System;
using System.Threading.Tasks;

namespace Rayn.Services.Realtime
{
    public interface IThreadRoomStore
    {
        ValueTask<IThreadRoom> FetchThreadRoomAsync(Guid threadId);
    }
}