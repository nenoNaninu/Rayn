using Microsoft.Extensions.DependencyInjection;
using Rayn.Services.Realtime;

namespace Rayn.Services.ServiceConfiguration
{
    public static class RealtimeThreadRoomDependencyInjectionExtensions
    {
        public static void AddRealtimeThreadRoomServices(this IServiceCollection services)
        {
            services.AddSingleton<IThreadRoomStore, ThreadRoomStore>();
            services.AddSingleton<IThreadRoomCreator, DefaultThreadRoomCreator>();
        }
    }
}