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

            var cfgOption = Configuration.GetSection(nameof(TelemetryOptions))
                .Get<TelemetryOptions>();

            if (cfgOption is null || string.IsNullOrWhiteSpace(cfgOption?.SourceName))
            {
                throw new ArgumentNullException(nameof(TelemetryOptions), "Options not found or value is incorrect!");
            }

            serviceCollection.Configure<TelemetryOptions>(
                opt => opt.SourceName = cfgOption.SourceName
            );

            source_name = new ActivitySource(cfgOption.SourceName).Name;

            serviceCollection.AddSingleton<ITelemetry, Telemetry>();

            return serviceCollection;
        }

    }
}
