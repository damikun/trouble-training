using System;
using Serilog;
using Serilog.Exceptions;
using Serilog.Enrichers.Span;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.SystemConsole.Themes;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;

namespace APIServer.Configuration
{
    public static partial class ServiceExtension
    {

        public static void ConfigureLogging(IHost host)
        {

            var logCfg = new LoggerConfiguration();

            using (var scope = host.Services.CreateScope())
            {

                var services = scope.ServiceProvider;

                var hostEnvironment = scope
                                        .ServiceProvider
                                        .GetRequiredService<IWebHostEnvironment>();

                var configuration = GetLogConfigurationFromJson();

                logCfg = logCfg
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithMachineName()
                    .Enrich.WithSpan()
                    .Enrich.WithElasticApmCorrelationInfo()
                    .Enrich.WithProperty(
                        "Environment",
                         hostEnvironment.EnvironmentName)
                    .ReadFrom.Configuration(configuration);

                try
                {

                    if (hostEnvironment.IsDevelopment())
                    {
                        logCfg = logCfg.WriteTo.Console(
                            theme: AnsiConsoleTheme.Code,
                            outputTemplate: "[{ElasticApmTraceId} {ElasticApmTransactionId} {Message:lj} {NewLine}{Exception}",
                            applyThemeToRedirectedOutput: true);
                    }

                    logCfg = logCfg.WriteTo.Elasticsearch(
                        ConfigureElasticSink(configuration, hostEnvironment.EnvironmentName));


                }
                catch { }
            }

            Log.Logger = logCfg.CreateLogger();
        }

        public static IConfigurationRoot GetLogConfigurationFromJson()
        {

            return new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile(
                    path: $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    optional: true)
                .Build();
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ConnectionStrings:Elasticsearch"]))
            {
                /// <summary>
                /// When set to true the sink will register an index template for the logs in elasticsearch.
                /// This template is optimized to deal with serilog events
                /// </summary>
                AutoRegisterTemplate = true,

                CustomFormatter = new EcsTextFormatter(),

                /// <summary>
                /// When using the <see cref="AutoRegisterTemplate"/> feature, this allows to set the Elasticsearch version. Depending on the
                /// version, a template will be selected. Defaults to pre 5.0.
                /// </summary>
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,

                ///<summary>
                /// Connection configuration to use for connecting to the cluster.
                /// </summary>
                ModifyConnectionSettings = configuration => configuration.ServerCertificateValidationCallback(
                    (o, certificate, arg3, arg4) => { return true; }),

                ///<summary>
                /// The index name formatter. A string.Format using the DateTimeOffset of the event is run over this string.
                /// defaults to "logstash-{0:yyyy.MM.dd}"
                /// Needs to be lowercased.
                /// </summary>
                IndexFormat = $"{Assembly.GetExecutingAssembly()?.GetName()?.Name?.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                // QueueSizeLimit = 1000,
                // ConnectionTimeout = new TimeSpan(0,5,0), // 5mins
                // BufferFileSizeLimitBytes = 5242880,
                // BufferLogShippingInterval = new TimeSpan(0,5,0), // 5mins
                // FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
            };
        }
    }
}
