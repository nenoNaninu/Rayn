using System.Threading.Tasks;
using Rayn.Services.Realtime.Models;

namespace Rayn.Services.Realtime.Hubs.Abstractions;

public interface IThreadRoomClient
{
    Task EnterRoomResultAsync(bool result, ThreadMessage[] message);
    Task ReceiveMessageFromServer(ThreadMessage message);
}
