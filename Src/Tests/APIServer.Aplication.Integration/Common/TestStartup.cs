using System;
using APIServer.Persistence;
using APIServer.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SharedCore.Aplication.Services;
using SharedCore.Aplication.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APIServer.Application.IntegrationTests
{
    public class TestStartup : Startup
    {
        public TestStartup(
            IWebHostEnvironment environment,
            IConfiguration configuration)
            : base(configuration, environment)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddHttpClient();

            services.AddHttpContextAccessor();

            services.AddPooledDbContextFactory<ApiDbContext>(
                (s, o) => o
                    // .UseSqlite("DataSource=:memory:"));
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddTelemetryService(Configuration, out string source);

            services.AddMapper();

            services.AddMediatR();

            services.AddAuthentication();

            services.AddAuthorization();

            services.AddSingleton(Serilog.Log.Logger);
        }

        public override void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider,
            IServiceScopeFactory scopeFactory)
        {
            app.UseEnsureApiContextCreated(serviceProvider, scopeFactory);

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization("ApiCaller");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

        }
    }
}