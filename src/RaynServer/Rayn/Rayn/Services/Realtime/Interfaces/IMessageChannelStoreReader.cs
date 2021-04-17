using System;

namespace Rayn.Services.Realtime.Interfaces
{
    /// <summary>
    /// For polling
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessageChannelStoreReader<T>
    {
        (bool isExist, IMessageChannel<T> messageChannel) GetMessageChannel(Guid threadId);
    }
}