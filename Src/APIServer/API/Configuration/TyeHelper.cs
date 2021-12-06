using SharedCore.Configuration;
using Microsoft.Extensions.Configuration;

namespace APIServer.Configuration
{
    public static partial class TyeExtension
    {
        public const string IdentityServerName = "identityserver";
        public const string BFFServerName = "bffserver";

#nullable enable
        public static string? GetIdentityServerUri(this IConfiguration config) =>
            TyeExtensionBase.GetServiceUrl(
                config,
                IdentityServerName,
                TyeExtensionBase.protocol) ?? config[$"ConnectionStrings:{IdentityServerName}"]?.TrimEnd('/');

        public static string? GetBFFServerUri(this IConfiguration config) =>
            TyeExtensionBase.GetServiceUrl(
                config,
                BFFServerName,
                TyeExtensionBase.protocol) ?? config[$"ConnectionStrings:{BFFServerName}"]?.TrimEnd('/');

#nullable disable

    }
}