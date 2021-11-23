using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Duende.IdentityServer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Configuration
{
    public static partial class ServiceExtension
    {
        public static IServiceCollection AddCorsConfiguration(
        this IServiceCollection serviceCollection,
        IWebHostEnvironment Environment)
        {

            string[] allowed_origins = null;

            if (Environment.IsDevelopment())
            {
                allowed_origins = new string[]{
                    "https://localhost:5001",
                    "https://localhost:5015",
                    "https://localhost:5021",
                    "https://localhost:5022",
                    "https://localhost:5070",
                    "https://localhost",
                    "host.docker.internal",
                    "http://localhost"
                };
            }
            else
            {
                // Add your production origins hire
                allowed_origins = new string[]{
                    "https://localhost:5001"
                };
            }

            serviceCollection.AddCors(options =>
            {
                options.AddPolicy("cors_policy", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    //------------------------------------
                    policy.WithOrigins(allowed_origins);
                    //policy.AllowAnyOrigin()
                    //------------------------------------
                    policy.AllowCredentials();
                    policy.SetPreflightMaxAge(TimeSpan.FromSeconds(10000));
                });
            });

            // This is IdentityServer part
            serviceCollection.AddSingleton<ICorsPolicyService>((container) =>
            {
                var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();

                return new DefaultCorsPolicyService(logger)
                {
                    AllowedOrigins = allowed_origins
                    //AllowAll = true
                };
            });

            return serviceCollection;

        }
    }
}