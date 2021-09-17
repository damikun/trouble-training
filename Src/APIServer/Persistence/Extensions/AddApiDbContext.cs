using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace APIServer.Persistence.Extensions {
    
    public static partial class ServiceExtension {

        public static IServiceCollection AddApiDbContext(
            this IServiceCollection serviceCollection,
            IConfiguration Configuration, IWebHostEnvironment Environment) {

            serviceCollection.AddPooledDbContextFactory<ApiDbContext>(
                (s, o) => o
                    .UseNpgsql(Configuration["ConnectionStrings:ApiDbContext"], option => {
                    option.EnableRetryOnFailure();

                    if (Environment.IsDevelopment()) {
                        o.EnableDetailedErrors();
                        o.EnableSensitiveDataLogging();
                    }

                    }).UseLoggerFactory(s.GetRequiredService<ILoggerFactory>()));
                    
            return serviceCollection;
        }
    }
}