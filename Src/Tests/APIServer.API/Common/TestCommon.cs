// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static APIServer.API.IntegrationTests.BaseClassFixture;

namespace APIServer.API.IntegrationTests
{

    public static class Common
    {

        public static TestUser GetDefaultUser()
        {
            return new TestUser()
            {
                UserName = "testuser",
                Password = "testuser"
            };
        }

        public static TestClinet GetDefaultClinet()
        {
            return new TestClinet()
            {
                ClinetId = "test",
                ClientSecret = "secret",
                Scope = "api"
            };
        }

    }

    public static class Extensions
    {
        public static async Task<QueryResponse> ProcessQuery(
            this HttpClient client,
            string query,
            object variables = null,
            IDictionary<string, string> headers = null
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

            var post_response = await client.PostAsync("/graphql", content);

            client.DefaultRequestHeaders.Clear();

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

    public class QueryResponse
    {
        public HttpResponseMessage raw_response { get; set; }

#nullable enable
        public string? data_content { get; set; }
#nullable disable
    }


}