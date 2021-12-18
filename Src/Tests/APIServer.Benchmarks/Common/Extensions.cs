
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace APIServer.Benchmark
{
    public static class Extensions
    {
        public static async Task<QueryResponse> ProcessQuery(
            this HttpClient client,
            string query,
            object? variables = null,
            IDictionary<string, string>? headers = null,
            string gql_endpoint = "graphql"
        )
        {

            if (variables == null)
            {
                variables = new { };
            }

            if (headers != null)
            {
                foreach (var header in headers.Where(e => e.Value != null))
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }

            var queryObject = new
            {
                query = query,
                variables = variables
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(queryObject),
                Encoding.UTF8,
                "application/json"
            );

            var post_response = await client.PostAsync(gql_endpoint, content);

            // client.DefaultRequestHeaders.Clear();

            string data_string = null;

            try
            {
                data_string = await post_response.Content.ReadAsStringAsync();
            }
            catch { }

            return new QueryResponse()
            {
                raw_response = post_response,
                data_content = data_string
            };
        }
    }
}