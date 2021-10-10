using Microsoft.Extensions.DependencyInjection;
using Rayn.Services.Realtime;
using Rayn.Services.Realtime.Interfaces;
using Rayn.Services.Realtime.Models;

namespace Rayn.Services.ServiceConfiguration
{
    public static class RealtimeDependencyInjectionExtensions
    {
        public static void AddRealtimeServices(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionGroupCache, ConnectionGroupCache>();

            var messageChannelStore = new MessageChannelStore<ThreadMessage>();
            services.AddSingleton<IMessageChannelStoreCreator<ThreadMessage>>(messageChannelStore);
            services.AddSingleton<IMessageChannelStoreReader<ThreadMessage>>(messageChannelStore);
        }
    }
}