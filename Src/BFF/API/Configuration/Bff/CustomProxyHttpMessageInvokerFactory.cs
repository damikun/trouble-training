using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using Duende.Bff;

namespace BFF.Configuration {

    public class CustomHttpMessageInvokerFactory : IHttpMessageInvokerFactory
    {

        protected readonly ConcurrentDictionary<string, HttpMessageInvoker> Clients = new();

        /// <inheritdoc />
        public virtual HttpMessageInvoker CreateClient(string localPath)
        {
            return Clients.GetOrAdd(localPath, (key) =>
            {

                return new CustomProxyHttpMessageInvoker(new SocketsHttpHandler
                {
                    UseProxy = false,
                    AllowAutoRedirect = false,
                    AutomaticDecompression = DecompressionMethods.None,
                    UseCookies = false
                });
            });
        }
    }
}