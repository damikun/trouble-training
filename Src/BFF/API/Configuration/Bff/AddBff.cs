using System.Collections.Generic;
using Duende.Bff;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BFF.Configuration
{
    public static partial class ServiceExtension {
        public static IServiceCollection AddBff(
        this IServiceCollection serviceCollection) {
    
            // Add BFF services to DI - also add server-side session management
            serviceCollection.AddBff(options =>
            {
                options.ForwardedHeaders = new HashSet<string>(){
                    "Correlation-Context",
                    "traceparent",
                    "tracestate",
                    "Request-Id"};
                options.AntiForgeryHeaderValue = "1";
                options.AntiForgeryHeaderName = "X-CSRF";
                options.ManagementBasePath = "/system";
            }).AddServerSideSessions();
            // .AddEntityFrameworkServerSideSessions(options=> 
            // {
            //     /// setup hire     
            // });

            serviceCollection.AddSingleton<IHttpMessageInvokerFactory, CustomHttpMessageInvokerFactory>();

            return serviceCollection;

        }
    }
}