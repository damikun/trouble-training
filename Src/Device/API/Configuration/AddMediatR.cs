using MediatR;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Device.Aplication.Shared.Behaviours;
using Device.Aplication.Commands.Test;

namespace Device.Configuration {
    public static partial class ServiceExtension {
        public static IServiceCollection AddMediatR(this IServiceCollection services) {

            services.AddMediatR(typeof(Trigger_Authorised).GetTypeInfo().Assembly);

            services.AddValidatorsFromAssembly(typeof(Trigger_AuthorisedValidator).GetTypeInfo().Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingBehaviour<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExBehaviour<,>));

            return services;
        }
    }
}