using System;
using HotChocolate;
using System.Threading;
using HotChocolate.Execution;
using System.Threading.Tasks;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SharedCore.Aplication.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace APIServer.Aplication.GraphQL.Extensions
{

    public class IntrospectionInterceptor : DefaultHttpRequestInterceptor
    {
        private readonly IWebHostEnvironment _env;

        public IntrospectionInterceptor(
            [Service] IWebHostEnvironment env
            )
        {
            _env = env;
        }

        public override ValueTask OnCreateAsync(HttpContext context,
            IRequestExecutor requestExecutor, IQueryRequestBuilder requestBuilder,
            CancellationToken cancellationToken)
        {

            if (!_env.IsDevelopment())
            {

                if (DoesUserExist(context))
                {
                    requestBuilder.AllowIntrospection();
                }
                else
                {
                    requestBuilder.SetIntrospectionNotAllowedMessage(
                        "Only logged-in users can introspect the server");
                }
            }

            return base.OnCreateAsync(context, requestExecutor, requestBuilder,
                cancellationToken);
        }

        private static bool DoesUserExist(HttpContext context)
        {
            try
            {
                return context?.User?.GetId<Guid>() != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
