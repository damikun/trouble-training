using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace BFF.Configuration
{
    public static partial class ServiceExtension
    {

        public static IServiceCollection AddFwdHeaders(
            this IServiceCollection serviceCollection)
        {
            return serviceCollection.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

    }
}