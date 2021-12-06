using System;
using SharedCore.Configuration;
using Microsoft.Extensions.Configuration;

namespace BFF.Configuration
{
    public static class TyeExtension
    {
        public const string APIServerName = "apiserver";

        public const string OtelCollector = "OtelCollector";

        public const string IdentityServerName = "identityserver";


#nullable enable

        public static string? GetAPIServerUri(this IConfiguration config) =>
            TyeExtensionBase.GetServiceUrl(
                config,
                APIServerName,
                TyeExtensionBase.protocol) ?? new Uri(config[$"ConnectionStrings:{APIServerName}"])?.ToString()?.TrimEnd('/');

        public static string? GetIdentityServerUri(this IConfiguration config) =>
            TyeExtensionBase.GetServiceUrl(
                config,
                IdentityServerName,
                TyeExtensionBase.protocol) ?? config[$"ConnectionStrings:{IdentityServerName}"]?.TrimEnd('/');

        public static string? GetOtelCollectorUri(this IConfiguration config) =>
            new Uri(config[$"ConnectionStrings:{OtelCollector}"])?.ToString()?.TrimEnd('/');

#nullable disable

    }
}