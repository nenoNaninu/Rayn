using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rayn.Services.Database.Abstractions;
using Rayn.Services.Database.InMemory;

namespace Rayn.Services.DependencyInjection;

public static class InMemoryModeDependencyInjectionExtensions
{
    public static void AddInMemoryDatabaseServices(this IServiceCollection services)
    {
        services.TryAddSingleton<MemoryDatabase>();
        services.TryAddSingleton<ICommentAccessor, MemoryCommentAccessor>();
        services.TryAddSingleton<IThreadCreator, MemoryThreadCreator>();
        services.TryAddSingleton<IThreadDbReader, MemoryThreadReader>();
        services.TryAddSingleton<IAccountRegister, MemoryAccountRegister>();
        services.TryAddSingleton<IGoogleAccountRegister, MemoryGoogleAccountRegister>();
        services.TryAddSingleton<IGoogleAccountReader, MemoryGoogleAccountReader>();
    }
}
