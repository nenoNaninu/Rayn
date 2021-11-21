using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rayn.Services.Realtime;
using Rayn.Services.Realtime.Interfaces;
using Rayn.Services.Realtime.Models;

namespace Rayn.Services.ServiceConfiguration;

public static class RealtimeDependencyInjectionExtensions
{
    public static void AddRealtimeServices(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionGroupCache, ConnectionGroupCache>();

        var messageChannelStore = new MessageChannelStore<ThreadMessage>();
        services.TryAddSingleton<IMessageChannelStoreCreator<ThreadMessage>>(messageChannelStore);
        services.TryAddSingleton<IMessageChannelStoreReader<ThreadMessage>>(messageChannelStore);
    }
}
