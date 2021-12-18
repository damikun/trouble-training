using System;
using IdentityServer.Persistence;
using Microsoft.AspNetCore.Hosting;
using IdentityServer.Configuration;
using IdentityServer.Domain.Models;
using Microsoft.AspNetCore.Builder;
using SharedCore.Aplication.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APIServer.Benchmark
{
    public class IdentityServerStartup : IdentityServer.API.Startup
    {
        private readonly IConfiguration _cfg;
        public IdentityServerStartup(
            IWebHostEnvironment environment,
            IConfiguration configuration)
            : base(configuration, environment)
        {
            _cfg = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsConfiguration(Environment, _cfg);

            services.AddControllersWithViews();

            services.AddDbContext<AppIdnetityDbContext>(option =>
            {
                option.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });

            services.AddHealthChecks();

            services.AddHealthChecks();

            services.AddMvc();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<AppIdnetityDbContext>()
            .AddDefaultTokenProviders();

            var identityServerBuilder = services.AddIdentityServer(
                options => options.KeyManagement.Enabled = false)
                .AddInMemoryClients(Clients.Get())
                .AddInMemoryIdentityResources(Resources.GetIdentityResources())
                .AddInMemoryApiResources(Resources.GetApiResources())
                .AddInMemoryApiScopes(Resources.GetApiScopes())
                .AddTestUsers(Users.Get());

            identityServerBuilder.AddDeveloperSigningCredential();

            services.AddTelemetryService(Configuration, out string trace_source);
        }

        public override void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();

            app.UseHealthChecks("/health");

            app.UseCookiePolicy();

            app.UseCors("cors_policy");

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{Action=Index}/{id?}");
            });
        }
    }
}