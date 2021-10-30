using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using System;
using Microsoft.AspNetCore.Http;
using SharedCore.Aplication.Interfaces;
using SharedCore.Aplication.Services;

namespace BFF.Configuration {
    public static partial class ServiceExtension {

        public static IServiceCollection AddTelemerty(
            this IServiceCollection serviceCollection,
            IConfiguration Configuration, IWebHostEnvironment Environment) {

            serviceCollection.AddTelemetryService(Configuration, out string source);
            
            serviceCollection.AddOpenTelemetryTracing((builder) => {
                // Sources
                builder.AddSource(source);

                builder.SetResourceBuilder(ResourceBuilder
                  .CreateDefault()
                //   .AddAttributes( new List<KeyValuePair<String, object>> { 
                //     new KeyValuePair<String, object>("SomeKey", "This is String Value")
                //     })
                  .AddService(Environment.ApplicationName));

                builder.AddAspNetCoreInstrumentation(opts => {
                    opts.RecordException = true;
                    opts.Enrich = async (activity, eventName, rawObject) =>
                    {

                        if (eventName.Equals("OnStartActivity"))
                        {
                            if (rawObject is HttpRequest {Path: {Value: "/graphql"}})
                            {
                            //     var req = rawObject as HttpRequest;

                            //     await SharedCore.Aplication.Shared.Common
                            //         .HandleTracingActivityRename(req);    
                            }
                        }
                    };
                });

                builder.AddElasticsearchClientInstrumentation();

                builder.AddSqlClientInstrumentation();

                builder.AddHttpClientInstrumentation(opts => opts.RecordException = true);

                builder.AddEntityFrameworkCoreInstrumentation(e => e.SetDbStatementForText = true);

                builder.AddOtlpExporter(options => {
                    options.Endpoint = new Uri(Configuration["ConnectionStrings:OtelCollector"]);             
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

            serviceCollection.AddSingleton<ITelemetry,Telemetry>();

            return serviceCollection;
        }
    }
}