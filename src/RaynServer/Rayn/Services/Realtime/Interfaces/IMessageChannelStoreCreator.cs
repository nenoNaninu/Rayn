using System;

namespace Rayn.Services.Realtime.Interfaces;

public interface IMessageChannelStoreCreator<T>
{
    void Create(Guid threadId);
}