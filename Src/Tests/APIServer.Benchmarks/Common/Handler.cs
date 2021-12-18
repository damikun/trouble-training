using System;
using System.Net.Http;
using System.Threading;
using IdentityModel.Client;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace APIServer.Benchmark.Common
{
    public class Handler
    {
        private Uri APIServerUri { get; set; }

        private Uri IdentityServerUri { get; set; }

        public HttpClient _client { get; set; } = new HttpClient();

        private static SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public string? Token { get; private set; }

        public AuthenticationHeaderValue AuthHeader
        {
            get
            {
                return new AuthenticationHeaderValue("Bearer", Token);
            }
        }

        public Handler(Uri api_server, Uri identity_server)
        {
            APIServerUri = api_server;

            IdentityServerUri = identity_server;
        }

        public static TestUser GetDefaultUser()
        {
            return new TestUser()
            {
                UserName = Cfg.DefaultUser,
                Password = Cfg.DefaulUserPassword
            };
        }

        public static TestClinet GetDefaultClinet()
        {
            return new TestClinet()
            {
                ClinetId = Cfg.OIDC_ClientId,
                ClientSecret = Cfg.OIDC_ClientSecret,
                Scope = Cfg.OIDC_ClientScope,
            };
        }

        public async Task RunAs(
           TestUser user_credentials,
           TestClinet client_credentials)
        {
            try
            {
                await _semaphore.WaitAsync();

                var token = await GetAccessTokenForUser(
                    user_credentials.UserName,
                    user_credentials.Password,
                    client_credentials.ClinetId,
                    client_credentials.ClientSecret,
                    client_credentials.Scope
               );

                Token = token;

                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Token);

            }
            finally
            {
                _semaphore.Release();
            }

        }

        private async Task<string> GetAccessTokenForUser(
            string userName,
            string password,
            string clientId,
            string clientSecret,
            string scope)
        {
            _client.DefaultRequestHeaders.Authorization = null;

            TokenClient tokenClient = new TokenClient(
                _client,
                 new TokenClientOptions
                 {
                     ClientId = clientId,
                     ClientSecret = clientSecret
                 });

            _client.BaseAddress = IdentityServerUri;

            var disco = await _client.GetDiscoveryDocumentAsync();

            if (disco is null || disco.TokenEndpoint is null)
            {
                throw new System.Exception("Failed to obtain data from discovery endpoint");
            }

            var response = await _client.RequestPasswordTokenAsync(
                new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,

                    UserName = userName,
                    Password = password,

                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    Scope = scope,
                });

            if (response is null || response.AccessToken is null)
            {
                throw new System.Exception("Resolved token from IdentityServer is null");
            }

            return response.AccessToken;
        }

    }
}
