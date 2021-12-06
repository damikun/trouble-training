using System;
using Microsoft.Extensions.Configuration;
using SharedCore.Configuration;

namespace IdentityServer.Configuration
{
    public static class TyeExtension
    {
        public const string APIServerName = "apiserver";

        public const string OtelCollector = "OtelCollector";

        public const string IdentityServerName = "identityserver";

        public const string BFFServerName = "bffserver";


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
               TyeExtensionBase.protocol) ?? config[$"ConnectionStrings:{IdentityServerName}"];

        public static string? GetBFFServerUri(this IConfiguration config) =>
            TyeExtensionBase.GetServiceUrl(
                config,
                BFFServerName,
                TyeExtensionBase.protocol) ?? config[$"ConnectionStrings:{BFFServerName}"];

        public static string? GetOtelCollectorUri(this IConfiguration config) =>
            new Uri(config[$"ConnectionStrings:{OtelCollector}"])?.ToString()?.TrimEnd('/');

#nullable disable

    }
}