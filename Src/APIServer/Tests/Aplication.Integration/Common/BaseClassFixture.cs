using Xunit;
using Microsoft.AspNetCore.TestHost;

namespace APIServer.Application.IntegrationTests
{
    public class BaseClassFixture : IClassFixture<XunitFixture>
    {
        protected readonly TestServer TestServer;

        public BaseClassFixture(XunitFixture fixture)
        {
            TestServer = fixture.TestServer;
        }
    }
}