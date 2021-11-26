using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rayn.Services.Database.Abstractions;
using Rayn.Services.Database.MySql;

namespace Rayn.Services.DependencyInjection;

public static class MySqlModeDependencyInjectionExtensions
{
    public static void AddMySqlDatabaseServices(this IServiceCollection services)
    {
        services.TryAddTransient<IThreadDbReader, MySqlThreadReader>();
        services.TryAddTransient<IThreadCreator, MySqlThreadCreator>();
        services.TryAddTransient<ICommentAccessor, MySqlCommentAccessor>();
        services.TryAddTransient<IAccountRegister, MySqlAccountRegister>();
        services.TryAddTransient<IGoogleAccountRegister, MySqlGoogleAccountRegister>();
        services.TryAddTransient<IGoogleAccountReader, MySqlGoogleAccountReader>();
    }
}
