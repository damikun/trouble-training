using System;
using APIServer.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APIServer.Application.IntegrationTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IWebHostEnvironment environment, IConfiguration configuration) 
            : base(configuration,environment)
        {
            
        }

        public override void ConfigureDBContext(IServiceCollection services)
        {
            services.AddPooledDbContextFactory<ApiDbContext>(
                (s, o) => o
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()));
        }

        public override void ConfigureAuth(IServiceCollection services)
        {
   
        }

        public override void ConfigureTelemetry(IServiceCollection services)
        {

        }

        public override void ConfigureScheduler(IServiceCollection services)
        {

        }
    }
}