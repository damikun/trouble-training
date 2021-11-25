using Xunit;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using System.Threading.Tasks;
using IdentityModel.Client;
using System.Net.Http.Headers;

namespace APIServer.API.IntegrationTests
{
    public class BaseClassFixture : IClassFixture<XunitFixture>
    {
        protected readonly TestServer IdentityTestServer;
        protected readonly TestServer APITestServer;
        protected readonly HttpClient HttpClient;
        protected readonly HttpClient TokenClient;
        private HttpMessageHandler _MessageHandler;

        public BaseClassFixture(XunitFixture fixture)
        {

            IdentityTestServer = fixture.IdentityTestServer;

            APITestServer = fixture.APITestServer;

            HttpClient = fixture.HttpClient;

            _MessageHandler = IdentityTestServer.CreateHandler();

            TokenClient = fixture.TokenClient;
        }

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

        public async Task RunAs(
            TestUser user_credentials,
            TestClinet client_credentials)
        {

            var token = await GetAccessTokenForUser(
                user_credentials.UserName,
                user_credentials.Password,
                client_credentials.ClinetId,
                client_credentials.ClientSecret,
                client_credentials.Scope
                );

            if (token != null)
            {
                HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                throw new System.Exception("Resolved token from IdentityServer is null");
            }

        }

        public async Task<string> GetAccessTokenForUser(
            string userName,
            string password,
            string clientId = "test",
            string clientSecret = "secret",
            string scope = "api")
        {

            TokenClient tokenClient = new TokenClient(
                TokenClient,
                 new TokenClientOptions
                 {
                     ClientId = clientId,
                     ClientSecret = clientSecret
                 });

            var disco = await TokenClient.GetDiscoveryDocumentAsync();

            var response = await TokenClient.RequestPasswordTokenAsync(
                new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,

                    UserName = userName,
                    Password = password,

                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    Scope = scope,
                });

            return response.AccessToken;
        }
    }
}