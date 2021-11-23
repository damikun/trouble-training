using IdentityServer.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Configuration
{
    public static partial class ServiceExtension
    {

        public static IServiceCollection AddAppIdentityDbContext(
            this IServiceCollection serviceCollection,
            IConfiguration Configuration, IWebHostEnvironment Environment)
        {

            serviceCollection.AddDbContext<AppIdnetityDbContext>(option =>
            {

                // option.UseNpgsql(Configuration["ConnectionStrings:AppIdnetityDbContext"], opt =>
                // {
                //     opt.EnableRetryOnFailure();
                // });

                option.UseSqlite("Data Source=../Persistence/api.db", opt =>
                {

                });

                if (Environment.IsDevelopment())
                {
                    option.EnableDetailedErrors();
                    option.EnableSensitiveDataLogging();
                }

            });


            return serviceCollection;

        }
    }
}
