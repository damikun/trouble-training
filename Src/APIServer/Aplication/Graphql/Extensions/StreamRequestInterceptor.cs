using HotChocolate;
using System.Threading;
using HotChocolate.Execution;
using System.Threading.Tasks;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace APIServer.Aplication.GraphQL.Extensions
{

    public class StreamRequestInterceptor : DefaultHttpRequestInterceptor
    {
        private readonly IWebHostEnvironment _env;

        public StreamRequestInterceptor(
            [Service] IWebHostEnvironment env
        )
        {
            _env = env;
        }

        public override ValueTask OnCreateAsync(HttpContext context,
            IRequestExecutor requestExecutor, IQueryRequestBuilder requestBuilder,
            CancellationToken cancellationToken)
        {
            // This is part of separate workshop and is not presen hire!

            return base.OnCreateAsync(context, requestExecutor, requestBuilder,
                cancellationToken);
        }
    }
}
