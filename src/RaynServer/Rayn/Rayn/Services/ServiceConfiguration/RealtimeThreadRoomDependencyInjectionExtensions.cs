using Microsoft.Extensions.DependencyInjection;
using Rayn.Services.Realtime;
using Rayn.Services.Realtime.Interfaces;

namespace Rayn.Services.ServiceConfiguration
{
    public static class RealtimeThreadRoomDependencyInjectionExtensions
    {
        public static void AddRealtimeThreadRoomServices(this IServiceCollection services)
        {
            services.AddSingleton<IConnectionGroupCache, ConnectionGroupCache>();
        }
    }
}