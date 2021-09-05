using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rayn.Services.Database.Configuration;
using Rayn.Services.Database.Interfaces;
using Rayn.Services.Realtime.Hubs;
using Rayn.Services.ServiceConfiguration;

namespace Rayn
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DapperConfiguration.Configure();

            IDatabaseConfig dbConfig = this.Configuration.GetSection("DatabaseConfig").Get<DatabaseConfiguration>();
            services.AddSingleton(dbConfig);


            services.AddControllersWithViews()
#if DEBUG
                .AddRazorRuntimeCompilation()
#endif
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                });

            if (dbConfig.InMemoryMode)
            {
                services.AddMemoryDatabaseModeServices();
            }
            else
            {
                services.AddMySqlDatabaseModeServices();
            }

            services.AddRealtimeServices();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");

                endpoints.MapHub<ThreadRoomHub>(ThreadRoomHub.Path);
            });
        }
    }
}
