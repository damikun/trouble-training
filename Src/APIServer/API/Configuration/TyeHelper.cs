using System;
using Microsoft.Extensions.Configuration;

namespace APIServer.Configuration
{
    public static class TyeExtension
    {
        public const string IdentityServerName = "identityserver";
        public const string BFFServerName = "bffserver";

#nullable enable
        public static string? GetServiceUrl(
            this IConfiguration config, string service_name, string binding)
        {
            // Temporary uncommented since bug in Tye (https://github.com/dotnet/tye/issues/697)
            // if (!IsTyeEnviroment(config))
            //     return null;

            return config.GetServiceUri(
                service_name,
                !string.IsNullOrWhiteSpace(binding) ? binding : null
            )?.AbsoluteUri.TrimEnd('/') ?? null;
        }

        public static string? GetIdentityServerUri(this IConfiguration config) =>
            GetServiceUrl(
                config,
                IdentityServerName,
                "https") ?? config[$"ConnectionStrings:{IdentityServerName}"]?.TrimEnd('/');

        public static string? GetBFFServerUri(this IConfiguration config) =>
            GetServiceUrl(
                config,
                BFFServerName,
                "https") ?? config[$"ConnectionStrings:{BFFServerName}"]?.TrimEnd('/');

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