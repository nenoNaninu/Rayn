using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rayn.Services.Realtime;
using Rayn.Services.Realtime.Abstractions;
using Rayn.Services.Realtime.Models;

namespace Rayn.Services.DependencyInjection;

public static class RealtimeDependencyInjectionExtensions
{
    public static void AddRealtimeServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IConnectionGroupCache, ConnectionGroupCache>();

        var messageChannelStore = new MessageChannelStore<ThreadMessage>();

        services.TryAddSingleton<IMessageChannelStoreCreator<ThreadMessage>>(messageChannelStore);
        services.TryAddSingleton<IMessageChannelStoreReader<ThreadMessage>>(messageChannelStore);
    }
}
