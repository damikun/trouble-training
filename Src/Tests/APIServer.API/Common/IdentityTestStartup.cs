using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using SharedCore.Aplication.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer.Configuration;
using Microsoft.AspNetCore.HttpOverrides;
using IdentityServer.Persistence;
using Microsoft.EntityFrameworkCore;
using IdentityServer.Domain.Models;
using Microsoft.AspNetCore.Identity;
using IdentityServer.API;

namespace APIServer.API.IntegrationTests
{
    public class IdentityTestStartup : IdentityServer.API.Startup
    {
        public IdentityTestStartup(
            IWebHostEnvironment environment,
            IConfiguration configuration)
            : base(configuration, environment)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {

            services.AddCorsConfiguration(Environment);

            services.AddControllersWithViews();

            services.AddDbContext<AppIdnetityDbContext>(option =>
            {

                option.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });

            services.AddHealthChecks();

            services.AddMvc();

            // app user 
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
                options => options.KeyManagement.Enabled = true)
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

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });

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