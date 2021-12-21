using System;
using System.IO;
using System.Threading;
using APIServer.Benchmark;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using APIServer.Benchmark.Common;

namespace APIServer.Benchmark
{
    public abstract class BenchmarkBase
    {
        public static readonly Uri APIServerUri = Cfg.APIServerUri;

        public static readonly Uri IdenitiyServerUri = Cfg.IdenitiyServerUri;

        public static readonly string Endpoint = Cfg.GraphqlEndpoint;

        private readonly APIServerHost _APIServer;

        private readonly IdentityServerHost _IdentityServer;

        CancellationToken _ct;

        Handler _handler;

        protected BenchmarkBase(CancellationToken ct)
        {
            _APIServer = new APIServerHost(APIServerUri);

            _IdentityServer = new IdentityServerHost(IdenitiyServerUri);

            _handler = new Handler(APIServerUri, IdenitiyServerUri);

            _ct = ct;
        }

        [GlobalSetup]
        public async Task StartEnviroment()
        {
            await Task.WhenAll(
                _APIServer.StartServerAsync(_ct),
                _IdentityServer.StartServerAsync(_ct)
            );

            Log("Waiting for hosts");
            await Task.WhenAll(
                _APIServer.WaitForHost(),
                _IdentityServer.WaitForHost()
            );

            Log("Requesting Access Token");
            await _handler.RunAs(
                Handler.GetDefaultUser(),
                Handler.GetDefaultClinet()
            );

            if (_handler.Token is null)
            {
                throw new ArgumentNullException("Failed to obtain access token from oidc provider!");
            }
        }

        [GlobalCleanup]
        public async Task StopEnviroment()
        {
            Log("Stopping all servers");
            await Task.WhenAll(
                _APIServer.StopServerAsync(_ct),
                _IdentityServer.StopServerAsync(_ct)
            );
        }

        public Task<QueryResponse> SendQueryAsync(string query, object? variables = null)
        {
            System.UriBuilder uriBuilder = new System.UriBuilder(APIServerUri);
            uriBuilder.Path += Endpoint;

            return _handler._client.ProcessQuery(query, variables, null, uriBuilder.Uri.ToString());
        }

        public virtual void Log(string message)
        {
            System.Console.WriteLine(message);
        }

    }
}