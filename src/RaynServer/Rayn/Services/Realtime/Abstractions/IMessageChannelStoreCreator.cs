using System;

namespace Rayn.Services.Realtime.Abstractions;

public interface IMessageChannelStoreCreator<T>
{
    void Create(Guid threadId);
}
