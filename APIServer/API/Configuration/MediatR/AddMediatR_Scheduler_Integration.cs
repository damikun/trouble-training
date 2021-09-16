using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace APIServer.Configuration {
    public static partial class ServiceExtension {
        public static IServiceCollection AddMediatRSchedulerIntegration(this IServiceCollection services) {

            services.AddSingleton<CommandsExecutorOptions>();
            services.AddScoped<CommandsExecutor>();
            services.AddScoped<AsyncCommand>();
            return services;
        }
    }
}