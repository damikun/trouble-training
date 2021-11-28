using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.Net.Security;

namespace APIServer.Configuration
{
    public static partial class ServiceExtension
    {

        public static IServiceCollection AddSSLCertHanlder(
            this IServiceCollection serviceCollection,
            IWebHostEnvironment env)
        {
            // This can be used to temporary dont validate certs!
            // This is just example!
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) =>
            {
                // local dev, just approve all certs
                if (env.IsDevelopment()) return true;

                return errors == SslPolicyErrors.None;
            };

            return serviceCollection;
        }

    }
}