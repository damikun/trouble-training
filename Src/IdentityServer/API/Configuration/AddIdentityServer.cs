using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using IdentityServer.Domain.Models;
using Microsoft.AspNetCore.Identity;
using IdentityServer.API;
using IdentityServer.Persistence;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using System.Linq;

namespace IdentityServer.Configuration
{
    public static partial class ServiceExtension
    {
        public static IServiceCollection AddIdentityServer(
        this IServiceCollection serviceCollection,
        IConfiguration Configuration,
        IWebHostEnvironment Environment)
        {

            // app user 
            serviceCollection.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<AppIdnetityDbContext>()
            .AddDefaultTokenProviders();

            var identityServerBuilder = serviceCollection.AddIdentityServer(options => options.KeyManagement.Enabled = true);

            if (Environment.IsDevelopment())
            {
                identityServerBuilder.AddDeveloperSigningCredential();
            }

            // codes, tokens, consents
            identityServerBuilder.AddOperationalStore<AppPersistedGrantDbContext>(options =>
                options.ConfigureDbContext = builder =>
                    // builder.UseNpgsql(Configuration["ConnectionStrings:AppIdnetityDbContext"]));
                    builder.UseSqlite("Data Source=../Persistence/api.db"));

            // clients, resources
            identityServerBuilder.AddConfigurationStore<AppConfigurationDbContext>(options =>
                options.ConfigureDbContext = builder =>
                     // builder.UseNpgsql(Configuration["ConnectionStrings:AppIdnetityDbContext"]))
                     builder.UseSqlite("Data Source=../Persistence/api.db"));

            identityServerBuilder.AddAspNetIdentity<ApplicationUser>();
            // serviceCollection.AddScoped<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();

            return serviceCollection;

        }

        /// <summary>
        /// A small bootstrapping method that will run EF migrations against the database and create your test data.
        /// </summary>
        public static void InitializeDbTestData(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var DB_PersistedGrantDbContext = serviceScope.ServiceProvider.GetRequiredService<AppPersistedGrantDbContext>();
                DB_PersistedGrantDbContext.Database.EnsureCreated();
                // DB_PersistedGrantDbContext.Database.Migrate();

                var DB_ConfigurationDbContext = serviceScope.ServiceProvider.GetRequiredService<AppConfigurationDbContext>();
                DB_ConfigurationDbContext.Database.EnsureCreated();
                // DB_ConfigurationDbContext.Database.Migrate();

                var DB_AppDbContext = serviceScope.ServiceProvider.GetRequiredService<AppIdnetityDbContext>();
                DB_AppDbContext.Database.EnsureCreated();
                // DB_AppDbContext.Database.Migrate();

                if (!DB_ConfigurationDbContext.Clients.Any())
                {
                    foreach (var client in Clients.Get())
                    {
                        DB_ConfigurationDbContext.Clients.Add(client.ToEntity());
                    }
                    DB_ConfigurationDbContext.SaveChanges();
                }

                if (!DB_ConfigurationDbContext.IdentityResources.Any())
                {
                    foreach (var resource in Resources.GetIdentityResources())
                    {
                        DB_ConfigurationDbContext.IdentityResources.Add(resource.ToEntity());
                    }
                    DB_ConfigurationDbContext.SaveChanges();
                }

                if (!DB_ConfigurationDbContext.ApiScopes.Any())
                {
                    foreach (var scope in Resources.GetApiScopes())
                    {
                        DB_ConfigurationDbContext.ApiScopes.Add(scope.ToEntity());
                    }
                    DB_ConfigurationDbContext.SaveChanges();
                }

                if (!DB_ConfigurationDbContext.ApiResources.Any())
                {
                    foreach (var resource in Resources.GetApiResources())
                    {
                        DB_ConfigurationDbContext.ApiResources.Add(resource.ToEntity());
                    }
                    DB_ConfigurationDbContext.SaveChanges();
                }

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                if (!userManager.Users.Any())
                {
                    foreach (var testUser in Users.Get())
                    {
                        var identityUser = new ApplicationUser(testUser.Username)
                        {
                            Id = testUser.SubjectId
                        };

                        userManager.CreateAsync(identityUser, "testuser").Wait();
                        userManager.AddClaimsAsync(identityUser, testUser.Claims.ToList()).Wait();
                    }
                }
            }
        }
    }
}