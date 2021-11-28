using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace SharedCore.Configuration
{
    public static partial class ServiceExtension
    {

        public static IServiceCollection AddSwagger(
            this IServiceCollection serviceCollection)
        {

            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Api",
                    Version = "v1"
                });
            });

            return serviceCollection;
        }

    }
}