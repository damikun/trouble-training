using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace APIServer.API.IntegrationTests
{
    public class XunitFixture : IDisposable
    {
        public TestServer APITestServer;
        public TestServer IdentityTestServer;
        public HttpClient HttpClient { get; }
        public HttpClient TokenClient { get; }
        public HttpMessageHandler HttpHandler { get; }

        public XunitFixture()
        {

            //----------------
            // Identity server
            //----------------

            var identity_builder = new WebHostBuilder()
            .ConfigureAppConfiguration((ctx, cfg) =>
            {
                cfg.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                cfg.AddJsonFile("serilog.json", optional: true, reloadOnChange: true);
            })
            .UseStartup<IdentityTestStartup>()
            .UseKestrel(options => options.Listen(IPAddress.Any, 5001));

            IdentityTestServer = new TestServer(identity_builder);

            // IdentityTestServer.BaseAddress = new Uri("https://localhost");

            IdentityTestServer.BaseAddress = new Uri("https://identityserver");

            TokenClient = IdentityTestServer.CreateClient();

            //----------------
            // Api server
            //----------------

            HttpHandler = IdentityTestServer.CreateHandler();

            ApiTestStartup.BackChannelHandler = HttpHandler;

            var api_builder = new WebHostBuilder()
                .ConfigureAppConfiguration((ctx, cfg) =>
                {
                    cfg.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    cfg.AddJsonFile("serilog.json", optional: true, reloadOnChange: true);
                })
                .UseStartup<ApiTestStartup>()
                .UseKestrel(options => options.Listen(IPAddress.Any, 5005));



            APITestServer = new TestServer(api_builder);



            HttpClient = APITestServer.CreateClient();
        }

        public void Dispose()
        {
            APITestServer.Dispose();

            HttpClient.Dispose();

            IdentityTestServer.Dispose();

            TokenClient.Dispose();

            HttpHandler.Dispose();
        }

    }
}