using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace BFF.Configuration
{

    public class CustomProxyHttpMessageInvoker : HttpMessageInvoker
    {

        public const string RequestIdHeaderName = "Request-Id";
        public const string CorrelationContextHeaderName = "Correlation-Context";
        public const string TraceParentHeaderName = "traceparent";
        public const string TraceStateHeaderName = "tracestate";

        public CustomProxyHttpMessageInvoker(HttpMessageHandler handler) : base(handler)
        {

        }

        public CustomProxyHttpMessageInvoker(HttpMessageHandler handler, bool disposeHandler) : base(handler, disposeHandler)
        {

        }

        private static HttpRequestMessage HandleTelemetryContextPropagation(HttpRequestMessage request)
        {
            var currentActivity = Activity.Current;

            if (currentActivity.IdFormat == ActivityIdFormat.W3C)
            {
                if (!request.Headers.Contains(TraceParentHeaderName))
                {
                    request.Headers.TryAddWithoutValidation(TraceParentHeaderName, currentActivity.Id);
                    if (currentActivity.TraceStateString != null)
                    {
                        request.Headers.TryAddWithoutValidation(TraceStateHeaderName, currentActivity.TraceStateString);
                    }
                }
            }
            else
            {
                if (!request.Headers.Contains(RequestIdHeaderName))
                {
                    request.Headers.TryAddWithoutValidation(RequestIdHeaderName, currentActivity.Id);
                }
            }

            // we expect baggage to be empty or contain a few items
            using (IEnumerator<KeyValuePair<string, string>> e = currentActivity.Baggage.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    var baggage = new List<string>();
                    do
                    {
                        KeyValuePair<string, string> item = e.Current;
                        baggage.Add(new NameValueHeaderValue(WebUtility.UrlEncode(item.Key), WebUtility.UrlEncode(item.Value)).ToString());
                    }
                    while (e.MoveNext());
                    request.Headers.TryAddWithoutValidation(CorrelationContextHeaderName, baggage);
                }
            }

            return request;
        }

        public override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            request = HandleTelemetryContextPropagation(request);

            return base.Send(request, cancellationToken);
        }


        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {

            request = HandleTelemetryContextPropagation(request);

            return base.SendAsync(request, cancellationToken);
        }

    }
}