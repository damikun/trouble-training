using System;
using Microsoft.Extensions.Configuration;

namespace BFF.Configuration
{
    public static class TyeExtension
    {
        public const string APIServerName = "apiserver";

        public const string OtelCollector = "OtelCollector";

        public const string IdentityServerName = "identityserver";

#nullable enable

        public static string? GetServiceUrl(
            this IConfiguration config, string service_name, string? binding)
        {
            if (!IsTyeEnviroment(config))
                return null;

            return config.GetServiceUri(
                service_name,
                !string.IsNullOrWhiteSpace(binding) ? binding : null
            )?.AbsoluteUri?.TrimEnd('/') ?? null;
        }

        public static string? GetAPIServerUri(this IConfiguration config) =>
            GetServiceUrl(
                config,
                APIServerName,
                "https") ?? new Uri(config[$"ConnectionStrings:{APIServerName}"])?.ToString()?.TrimEnd('/');

        public static string? GetIdentityServerUri(this IConfiguration config) =>
            GetServiceUrl(
                config,
                IdentityServerName,
                "https") ?? config[$"ConnectionStrings:{IdentityServerName}"]?.TrimEnd('/');

        public static string? GetOtelCollectorUri(this IConfiguration config) =>
            new Uri(config[$"ConnectionStrings:{OtelCollector}"])?.ToString()?.TrimEnd('/');

#nullable disable
        public static string GetHostUrl(this IConfiguration config)
        {
            var serviceName = config.GetValue<string>("Host:Name");
            return config.GetServiceUri(serviceName, "https")
                ?.AbsoluteUri?.TrimEnd('/') ?? config["Kestrel:Endpoints:Https:Url"];
        }

        public static bool IsTyeEnviroment(this IConfiguration config)
        {
            return config.GetHostUrl() is not null;
        }

    }
}