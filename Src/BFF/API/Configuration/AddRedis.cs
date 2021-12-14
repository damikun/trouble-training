using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BFF.Configuration
{
    public static partial class ServiceExtension
    {
        public static IServiceCollection AddRedis(
        this IServiceCollection serviceCollection, IConfiguration cfg)
        {
            // !This is not part of opensource workshop

            return serviceCollection;

        }
    }
}