using System;

namespace Rayn.Services.Realtime.Interfaces
{
    public interface IPollingUserConnectionCreator
    {
        IPollingUserConnection Create(Guid threadId, Guid ownerId);
    }
}