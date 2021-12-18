using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedCore.Aplication.Interfaces;

namespace SharedCore.Aplication.Services
{

    public static class TelemetryExtencions
    {

        public static IServiceCollection AddTelemetryService(
           this IServiceCollection serviceCollection,
           IConfiguration Configuration, out string source_name)
        {

            var identityOptions = Configuration.GetSection(nameof(TelemetryOptions))
                .Get<TelemetryOptions>();

            if (identityOptions is null)
            {
                throw new ArgumentNullException(nameof(TelemetryOptions), "Options not found!");
            }

            serviceCollection.Configure<TelemetryOptions>(
                opt => opt.SourceName = identityOptions.SourceName
            );

            source_name = new ActivitySource(identityOptions.SourceName).Name;

            serviceCollection.AddSingleton<ITelemetry, Telemetry>();

            return serviceCollection;
        }

    }
}
