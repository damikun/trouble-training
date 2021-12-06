using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

namespace BFF.Configuration
{
    public static partial class ServiceExtension
    {

        public static IServiceCollection AddSpa(
            this IServiceCollection serviceCollection)
        {
            // In production, the React files will be served from this directory
            serviceCollection.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            return serviceCollection;
        }

        public static void UseSpa(
            this IApplicationBuilder builder, IWebHostEnvironment env)
        {
            builder.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }



    }
}