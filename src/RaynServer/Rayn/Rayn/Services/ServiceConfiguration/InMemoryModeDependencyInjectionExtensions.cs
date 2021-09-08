using Microsoft.Extensions.DependencyInjection;
using Rayn.Services.Database.InMemory;
using Rayn.Services.Database.Interfaces;

namespace Rayn.Services.ServiceConfiguration
{
    public static class InMemoryModeDependencyInjectionExtensions
    {
        public static void AddMemoryDatabaseModeServices(this IServiceCollection services)
        {
            services.AddSingleton<MemoryDatabase>();
            services.AddSingleton<ICommentAccessor, MemoryCommentAccessor>();
            services.AddSingleton<IThreadCreator, MemoryThreadCreator>();
            services.AddSingleton<IThreadDbReader, MemoryThreadReader>();
            services.AddSingleton<IAccountRegister, MemoryAccountRegister>();
            services.AddSingleton<IGoogleAccountRegister, MemoryGoogleAccountRegister>();
            services.AddSingleton<IGoogleAccountReader, MemoryGoogleAccountReader>();
        }
    }
}