using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace APIServer.Configuration {
    public static partial class ServiceExtension {
        public static IServiceCollection MediatRScheduler(this IServiceCollection services) {

            services.AddSingleton<CommandsExecutorOptions>();
            services.AddScoped<CommandsExecutor>();
            services.AddScoped<AsyncCommand>();
            return services;
        }
    }
}