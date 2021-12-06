using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BFF.Configuration
{
    public static partial class ServiceExtension
    {

        private const bool csrf_protection_enabled = true;

        private const bool csrf_protection_disabled = false;


        public static IEndpointRouteBuilder MapRemoteEndpoints(
        this IEndpointRouteBuilder builder)
        {

            var cfg = builder.ServiceProvider.GetService<IConfiguration>();

            var env = builder.ServiceProvider.GetService<IWebHostEnvironment>();

            string api_base_url = cfg.GetAPIServerUri();

            builder.MapRemoteBffApiEndpoint(
                   "/traces",
                   "http://localhost:55680/v1/traces",
                   csrf_protection_enabled)
               .AllowAnonymous();

            builder.MapRemoteBffApiEndpoint(
                "/graphql",
                $"{api_base_url}/graphql",
                csrf_protection_enabled)
            .WithOptionalUserAccessToken()
            .AllowAnonymous();

            if (env.IsDevelopment())
            {
                builder.MapRemoteBffApiEndpoint(
                    "/playground",
                    $"{api_base_url}/playground",
                    csrf_protection_disabled)
                .WithOptionalUserAccessToken()
                .AllowAnonymous();

                builder.MapRemoteBffApiEndpoint(
                    "/voyager",
                    $"{api_base_url}/voyager",
                    csrf_protection_disabled)
                .WithOptionalUserAccessToken()
                .AllowAnonymous();

                builder.MapRemoteBffApiEndpoint(
                    "/bcp",
                    $"{api_base_url}/bcp",
                    csrf_protection_disabled)
                .WithOptionalUserAccessToken()
                .AllowAnonymous();

                builder.MapRemoteBffApiEndpoint(
                    "/hookloopback",
                   $"{api_base_url}/api/Hook/hookloopback",
                    csrf_protection_disabled)
                .AllowAnonymous();

                builder.MapRemoteBffApiEndpoint(
                    "/reset",
                   $"{api_base_url}/api/Test/ClearDatabase",
                    csrf_protection_enabled)
                .AllowAnonymous();

                builder.MapRemoteBffApiEndpoint(
                    "/scheduler",
                    $"{api_base_url}/scheduler",
                    csrf_protection_enabled)
                .WithOptionalUserAccessToken()
                .AllowAnonymous();

                builder.MapRemoteBffApiEndpoint(
                    "/swagger",
                    $"{api_base_url}/swagger",
                    csrf_protection_enabled)
                .AllowAnonymous();
            }

            return builder;

        }
    }
}