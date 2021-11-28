using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

namespace APIServer.Configuration
{
    public static partial class ServiceExtension
    {

        public static IApplicationBuilder UseFwdHeaders(
            this IApplicationBuilder builder)
        {
            return builder.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto |
                ForwardedHeaders.XForwardedHost
            });
        }

    }
}