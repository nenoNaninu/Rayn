using Microsoft.Extensions.DependencyInjection;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Database.MySql;

namespace Rayn.Services.ServiceConfiguration
{
    public static class MySqlModeDependencyInjectionExtensions
    {
        public static void AddMySqlDatabaseModeServices(this IServiceCollection services)
        {
            services.AddTransient<IThreadDbReader, MySqlThreadReader>();
            services.AddTransient<IThreadCreator, MySqlThreadCreator>();
            services.AddTransient<ICommentAccessor, MySqlCommentAccessor>();
        }
    }
}