using System;
using OpenTelemetry.Trace;
using System.Threading.Tasks;
using OpenTelemetry.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using SharedCore.Aplication.Services;
using SharedCore.Aplication.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SharedCore.Configuration
{
    public static partial class ServiceExtension
    {

        public static IServiceCollection AddTelemerty(
            this IServiceCollection serviceCollection,
            IConfiguration Configuration,
            IWebHostEnvironment Environment)
        {

            serviceCollection.AddTelemetryService(Configuration, out string trace_source);

            serviceCollection.AddOpenTelemetryTracing((builder) =>
            {
                // Sources
                builder.AddSource(trace_source);

                builder.SetResourceBuilder(ResourceBuilder
                  .CreateDefault()
                //   .AddAttributes( new List<KeyValuePair<String, object>> { 
                //     new KeyValuePair<String, object>("SomeKey", "This is String Value")
                //     })
                  .AddService(Environment.ApplicationName));

                builder.AddAspNetCoreInstrumentation(opts =>
                {
                    opts.RecordException = true;

                    // Enricher example
                    opts.Enrich = async (activity, eventName, rawObject) =>
                    {

                        await Task.CompletedTask;

                        if (eventName.Equals("OnStartActivity"))
                        {
                            if (rawObject is HttpRequest { Path: { Value: "/graphql" } })
                            {
                                // Do something with request..
                            }
                        }
                    };
                    // opts.Filter = (httpContext) =>
                    // {
                    // only collect telemetry about HTTP GET requests
                    // return httpContext.Request.Method.Equals("GET");
                    // };
                });

                // Uncommented since Net6 is not supported
                // builder.AddElasticsearchClientInstrumentation();

                builder.AddSqlClientInstrumentation();

                builder.AddHttpClientInstrumentation(
                    opts => opts.RecordException = true);

                builder.AddEntityFrameworkCoreInstrumentation(
                    e => e.SetDbStatementForText = true);

                builder.AddHotChocolateInstrumentation();

                builder.AddOtlpExporter(options =>
                {
                    // Export to collector
                    options.Endpoint = new Uri(Configuration["ConnectionStrings:OtelCollector"]);

                    options.TimeoutMilliseconds = 10000;

                    // Export dirrectly to APM
                    // options.Endpoint = new Uri("http://localhost:8200"); 
                    // options.BatchExportProcessorOptions = new OpenTelemetry.BatchExportProcessorOptions<Activity>() {
                    // };                
                });

                // This is example for Jaeger integration
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

            serviceCollection.AddLogging(opt =>
            {
                opt.AddTraceSource(trace_source);

                opt.AddOpenTelemetry(e =>
                {
                    e.IncludeFormattedMessage = true;

                    e.IncludeScopes = true;
                });
            });

            serviceCollection.AddScoped<ITelemetry, Telemetry>();

            return serviceCollection;
        }
    }
}