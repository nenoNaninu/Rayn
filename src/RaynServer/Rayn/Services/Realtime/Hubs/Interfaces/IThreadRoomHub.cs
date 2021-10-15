using System;
using System.Threading.Tasks;
using Rayn.Services.Realtime.Models;

namespace Rayn.Services.Realtime.Hubs.Interfaces
{
    public interface IThreadRoomHub
    {
        Task EnterThreadRoom(Guid threadId);
        Task PostMessageToServer(ThreadMessage message);
    }
}