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

namespace APIServer.Aplication.GraphQL.Extensions {

    public class StreamRequestInterceptor : DefaultHttpRequestInterceptor
    {
        private readonly IWebHostEnvironment _env;

        public StreamRequestInterceptor(
            [Service] IWebHostEnvironment env
            ){
            _env = env;
        }

        public override ValueTask OnCreateAsync(HttpContext context,
            IRequestExecutor requestExecutor, IQueryRequestBuilder requestBuilder,
            CancellationToken cancellationToken)
        {

            return base.OnCreateAsync(context, requestExecutor, requestBuilder,
                cancellationToken);
        }

        private static bool DoesUserExist(HttpContext context){
            try{
                return context?.User?.GetId<Guid>() != null;
            }catch{
                return false;
            }
        }
    }
}
