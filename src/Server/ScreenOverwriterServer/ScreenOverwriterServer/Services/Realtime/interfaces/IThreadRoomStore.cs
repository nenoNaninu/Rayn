using System;
using System.Threading.Tasks;

namespace ScreenOverwriterServer.Services.Realtime
{
    public interface IThreadRoomStore
    {
        ValueTask<IThreadRoom> FetchThreadRoomAsync(Guid threadId);
    }
}