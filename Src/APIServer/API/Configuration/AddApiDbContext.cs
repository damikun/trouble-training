
using Microsoft.AspNetCore.Hosting;
using APIServer.Persistence.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System;
using APIServer.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IdentityServer.Persistence;

namespace APIServer.Configuration {
    public static partial class ServiceExtension {

        public static IServiceCollection AddDbContext(
            this IServiceCollection serviceCollection,
             IConfiguration Configuration, IWebHostEnvironment Environment) {
                
                // ApiDbContext
                serviceCollection.AddApiDbContext(Configuration,Environment);

                serviceCollection.AddPooledDbContextFactory<AppIdnetityDbContext>(
                    (s, o) => o
                        .UseNpgsql(Configuration["ConnectionStrings:AppIdnetityDbContext"], option => {
                            
                            option.EnableRetryOnFailure();

                            if (Environment.IsDevelopment()) {
                                o.EnableDetailedErrors();
                                o.EnableSensitiveDataLogging();
                            }
                        }));

            return serviceCollection;
        }


        public static IApplicationBuilder UseEnsureApiContextCreated(
            this IApplicationBuilder app_builder,
            IServiceProvider serviceProvider, IServiceScopeFactory scopeFactory) {
                
            var serviceScopeFactory = app_builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

            using (var serviceScope = serviceScopeFactory.CreateScope()) {
                var _factory = serviceScope.ServiceProvider.GetService<IDbContextFactory<ApiDbContext>>();

                 using ApiDbContext dbContext =  _factory.CreateDbContext();

                dbContext.Database.EnsureCreated();
            }

            return app_builder;
        }
    }
}