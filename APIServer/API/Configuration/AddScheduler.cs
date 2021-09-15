using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APIServer.Configuration {
    public static partial class ServiceExtension {

        public static IServiceCollection AddScheduler(
            this IServiceCollection serviceCollection,
            IConfiguration Configuration) {

            serviceCollection.AddHangfire((provider, configuration) => {
                configuration.UsePostgreSqlStorage(Configuration["ConnectionStrings:HangfireConnection"]);
                configuration.UseFilter(new AutomaticRetryAttribute { Attempts = 5 });
            });

            // serviceCollection.AddHangfireServer(options => {
            //     options.Queues = new[] { "systemqueue", "default" };
            //     options.WorkerCount = 2;
            // });

            return serviceCollection;
        }
    }
}