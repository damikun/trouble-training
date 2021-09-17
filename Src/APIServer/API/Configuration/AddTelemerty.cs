using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using System;
using APIServer.Domain;

namespace APIServer.Configuration {
    public static partial class ServiceExtension {

        public static IServiceCollection AddTelemerty(
            this IServiceCollection serviceCollection,
            IConfiguration Configuration, IWebHostEnvironment Environment) {

            serviceCollection.AddOpenTelemetryTracing((builder) => {
                // Sources
                builder.AddSource(Sources.DemoSource.Name);

                builder.SetResourceBuilder(ResourceBuilder
                  .CreateDefault()
                //   .AddAttributes( new List<KeyValuePair<String, object>> { 
                //     new KeyValuePair<String, object>("SomeKey", "This is String Value")
                //     })
                  .AddService(Environment.ApplicationName));

                builder.AddAspNetCoreInstrumentation(opts => {
                    opts.RecordException = true;
                });

                builder.AddElasticsearchClientInstrumentation();

                builder.AddSqlClientInstrumentation();

                builder.AddHttpClientInstrumentation(opts => opts.RecordException = true);

                builder.AddEntityFrameworkCoreInstrumentation(e => e.SetDbStatementForText = true);

                builder.AddOtlpExporter(options => {
                    options.Endpoint = new Uri("http://localhost:55680"); // Export to collector
                    // options.Endpoint = new Uri("http://localhost:8200"); // Export dirrectly to APM
                    // options.BatchExportProcessorOptions = new OpenTelemetry.BatchExportProcessorOptions<Activity>() {
                    // };                
                });

                // if (Uri.TryCreate(Configuration.GetConnectionString("Jaeger"), UriKind.Absolute, out var uri)) {
                //     builder.AddJaegerExporter(opts => {
                //         opts.AgentHost = uri.Host;
                //         opts.AgentPort = uri.Port;
                //         opts.BatchExportProcessorOptions = new OpenTelemetry.BatchExportProcessorOptions<Activity>() {
                //         };
                //     });

                //     // builder.AddZipkinExporter(opts => {
                //     //     opts.Endpoint = new Uri("http://localhost:9412/api/v2/spans");
                //     // });
                // }
            });

            return serviceCollection;
        }
    }
}