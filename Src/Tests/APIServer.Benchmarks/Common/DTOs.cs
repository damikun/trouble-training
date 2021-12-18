using System.Net.Http;

namespace APIServer.Benchmark
{
    public class TestUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class TestClinet
    {
        public string ClinetId { get; set; }
        public string ClientSecret { get; set; }
        public string Scope { get; set; }
    }

    public class QueryResponse
    {
        public HttpResponseMessage raw_response { get; set; }
        public string? data_content { get; set; }
    }

}