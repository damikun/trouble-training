using Microsoft.Extensions.Configuration;

namespace SharedCore.Configuration
{
    public static class TyeExtensionBase
    {
        public const string protocol = "https";

#nullable enable

        public static string? GetServiceUrl(
            this IConfiguration config, string service_name, string binding)
        {
            if (!IsTyeEnviroment(config))
                return null;

            return config.GetServiceUri(
                service_name,
                !string.IsNullOrWhiteSpace(binding) ? binding : null
            )?.AbsoluteUri?.TrimEnd('/') ?? null;
        }


#nullable disable

        public static string GetHostUrl(this IConfiguration config)
        {
            var serviceName = config.GetValue<string>("Host:Name");
            return config.GetServiceUri(serviceName, protocol)?.AbsoluteUri?.TrimEnd('/') ?? config["Kestrel:Endpoints:Https:Url"]?.TrimEnd('/');
        }

        public static bool IsTyeEnviroment(this IConfiguration config)
        {
            return config.GetHostUrl() is not null;
        }

    }
}