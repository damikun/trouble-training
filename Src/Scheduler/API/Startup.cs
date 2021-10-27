using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hangfire.PostgreSql;
using APIServer.Configuration;

namespace Scheduler
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment enviroment)
        {
            Configuration = configuration;

            Environment = enviroment;
        }

        public IWebHostEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHangfire((provider, configuration) => {
                configuration.UsePostgreSqlStorage(Configuration.GetConnectionString("HangfireConnection"));
                configuration.UseFilter(new AutomaticRetryAttribute { Attempts = 5 });
            });

            services.AddHangfireServer(options => {
                options.Queues = new[] { "systemqueue", "default" };
                options.WorkerCount = 2;
            });

            services.AddMediatR();

            services.AddHttpClient();

            services.AddHealthChecks();

            services.AddDbContext(Configuration,Environment);

            services.AddHttpContextAccessor();

            services.AddSingleton(Serilog.Log.Logger);

            services.AddTelemerty(Configuration,Environment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()){
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/health");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
