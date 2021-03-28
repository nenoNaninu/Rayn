using Microsoft.Extensions.DependencyInjection;
using ScreenOverwriterServer.Services.Database;
using ScreenOverwriterServer.Services.Database.Interfaces;

namespace ScreenOverwriterServer.Services.DependencyInjection
{
    public static class InMemoryModeDependencyInjectionExtensions
    {
        public static void AddMemoryDatabaseModeSetting(this IServiceCollection services)
        {
            services.AddSingleton<MemoryDatabase>();
            services.AddSingleton<ICommentAccessor, MemoryCommentAccessor>();
            services.AddSingleton<IThreadCreator, MemoryThreadCreator>();
            services.AddSingleton<IThreadDbReader, MemoryThreadReader>();

        }
    }
}