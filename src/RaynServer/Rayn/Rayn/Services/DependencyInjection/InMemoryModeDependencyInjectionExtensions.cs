using Microsoft.Extensions.DependencyInjection;
using Rayn.Services.Database;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Realtime;

namespace Rayn.Services.DependencyInjection
{
    public static class InMemoryModeDependencyInjectionExtensions
    {
        public static void AddMemoryDatabaseModeServices(this IServiceCollection services)
        {
            services.AddSingleton<MemoryDatabase>();
            services.AddSingleton<ICommentAccessor, MemoryCommentAccessor>();
            services.AddSingleton<IThreadCreator, MemoryThreadCreator>();
            services.AddSingleton<IThreadDbReader, MemoryThreadReader>();
        }

        public static void AddRealtimeThreadRoomServices(this IServiceCollection services)
        {
            services.AddSingleton<IThreadRoomStore, ThreadRoomStore>();
            services.AddSingleton<IThreadRoomCreator, DefaultThreadRoomCreator>();
        }
    }
}