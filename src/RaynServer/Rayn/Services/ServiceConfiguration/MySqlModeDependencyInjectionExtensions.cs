using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Database.MySql;

namespace Rayn.Services.ServiceConfiguration;

public static class MySqlModeDependencyInjectionExtensions
{
    public static void AddMySqlDatabaseModeServices(this IServiceCollection services)
    {
        services.TryAddTransient<IThreadDbReader, MySqlThreadReader>();
        services.TryAddTransient<IThreadCreator, MySqlThreadCreator>();
        services.TryAddTransient<ICommentAccessor, MySqlCommentAccessor>();
        services.TryAddTransient<IAccountRegister, MySqlAccountRegister>();
        services.TryAddTransient<IGoogleAccountRegister, MySqlGoogleAccountRegister>();
        services.TryAddTransient<IGoogleAccountReader, MySqlGoogleAccountReader>();
    }
}
