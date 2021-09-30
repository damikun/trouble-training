using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using System;
using Device.Domain;
using Microsoft.AspNetCore.Http;
using SharedCore.Aplication.Extensions;

namespace Device.Configuration {
    public static partial class ServiceExtension {

        public static IServiceCollection AddTelemerty(
            this IServiceCollection serviceCollection,
            IConfiguration Configuration, IWebHostEnvironment Environment) {

            serviceCollection.AddOpenTelemetryTracing((builder) => {
                // Sources
                builder.AddSource(Sources.DemoSource.Name);

                builder.SetResourceBuilder(ResourceBuilder
                  .CreateDefault()
                  .AddService(Environment.ApplicationName));

                builder.AddAspNetCoreInstrumentation(opts => {
                    opts.RecordException = true;
                    opts.Enrich = async (activity, eventName, rawObject) =>
                    {

                        if (eventName.Equals("OnStartActivity"))
                        {
                            if (rawObject is HttpRequest {Path: {Value: "/graphql"}})
                            {
                                var req = rawObject as HttpRequest;

                                await Extensions.HandleTracingActivityRename(req);    
                            }
                        }
                    };
                });

                builder.AddElasticsearchClientInstrumentation();

                builder.AddSqlClientInstrumentation();

                builder.AddHttpClientInstrumentation(opts => opts.RecordException = true);

                builder.AddEntityFrameworkCoreInstrumentation(e => e.SetDbStatementForText = true);

                builder.AddOtlpExporter(options => {
                    options.Endpoint = new Uri("http://localhost:55680");             
                });
            });

            return serviceCollection;
        }
    }
}