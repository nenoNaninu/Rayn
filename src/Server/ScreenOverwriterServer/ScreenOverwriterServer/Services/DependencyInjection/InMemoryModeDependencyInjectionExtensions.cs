using Microsoft.Extensions.DependencyInjection;
using ScreenOverwriterServer.Services.Database;
using ScreenOverwriterServer.Services.Database.Interfaces;
using ScreenOverwriterServer.Services.Realtime;

namespace ScreenOverwriterServer.Services.DependencyInjection
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