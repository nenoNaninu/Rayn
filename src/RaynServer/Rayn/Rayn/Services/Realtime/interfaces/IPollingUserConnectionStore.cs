using System;

namespace Rayn.Services.Realtime.Interfaces
{
    public interface IPollingUserConnectionStore
    {
        IPollingUserConnection FetchPollingUserConnection(Guid ownerId);
        void Add(Guid ownerId, IPollingUserConnection connection);
    }
}