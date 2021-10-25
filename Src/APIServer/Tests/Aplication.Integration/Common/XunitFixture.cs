using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace APIServer.Application.IntegrationTests
{
    public class XunitFixture : IDisposable
    {
        public TestServer TestServer;

        public XunitFixture()
        {
            var builder = new WebHostBuilder()
                .ConfigureAppConfiguration((ctx, cfg) =>
                {
                    cfg.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    cfg.AddJsonFile("serilog.json", optional: true, reloadOnChange: true);
                })
                .UseStartup<TestStartup>();

            TestServer = new TestServer(builder);
        }

        public void Dispose()
        {
            TestServer.Dispose();
        }
        
    }
}