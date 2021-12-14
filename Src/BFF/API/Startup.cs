// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

using Duende.Bff.Yarp;
using BFF.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BFF
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
            services.AddCorsConfiguration(Environment, Configuration);

            services.AddControllersWithViews();

            services.AddRedis(Configuration);

            services.AddSpa();

            services.AddBff();

            services.AddIdentityConfiguration(Configuration);

            services.AddHealthChecks();

            services.ConfigureFwdHeaders();

            services.AddTelemerty(Configuration, Environment);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //app.UseElasticApm(Configuration, new IDiagnosticsSubscriber [0]);

            app.UseHealthChecks("/health");

            app.UseCors("cors_policy");

            app.UseHttpsRedirection();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseSpaStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseBff();

            app.UseAuthorization(); // Adds authorization for local and remote API endpoints

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers() // Local endpoints
                    .RequireAuthorization()
                    .AsBffApiEndpoint();

                endpoints.MapBffManagementEndpoints();   // login, logout, user, backchannel logout...

                endpoints.MapRemoteEndpoints(); // Remotes
            });

            app.UseSpa(env);
        }
    }
}
