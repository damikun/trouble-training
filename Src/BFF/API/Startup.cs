// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

using BFF.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using System.Net;
using System.Net.Security;
using Duende.Bff.Yarp;


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

            services.AddCorsConfiguration(Environment);

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddBff();

            services.AddIdentityConfiguration();

            services.AddHealthChecks();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddTelemerty(Configuration, Environment);

            // ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) =>
            // {
            //     // local dev, just approve all certs
            //     if (Environment.IsDevelopment()) return true;

            //     return errors == SslPolicyErrors.None;
            // };
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

            app.UseAuthorization(); // adds authorization for local and remote API endpoints

            app.UseEndpoints(endpoints =>
            {

                // local APIs
                endpoints.MapControllers()
                    .RequireAuthorization()
                    .AsBffApiEndpoint();

                endpoints.MapBffManagementEndpoints();   // login, logout, user, backchannel logout...

                bool csrf_protection_enabled = !env.IsDevelopment();

                // proxy endpoint for cross-site APIs
                // all calls to /api/* will be forwarded to the remote API
                // user or client access token will be attached in API call
                // user access token will be managed automatically using the refresh token
                endpoints.MapRemoteBffApiEndpoint(
                    "/graphql",
                    "https://localhost:5022/graphql",
                    csrf_protection_enabled)
                .WithOptionalUserAccessToken()
                .AllowAnonymous();

                endpoints.MapRemoteBffApiEndpoint(
                    "/traces",
                    "http://localhost:55690/v1/traces",
                    csrf_protection_enabled)
                .AllowAnonymous();

                if (env.IsDevelopment())
                {

                    endpoints.MapRemoteBffApiEndpoint(
                        "/hookloopback",
                        "https://localhost:5022/api/Hook/hookloopback",
                        false)
                    .AllowAnonymous();

                    endpoints.MapRemoteBffApiEndpoint(
                        "/reset",
                        "https://localhost:5022/api/Test/ClearDatabase",
                        false)
                    .AllowAnonymous();

                    endpoints.MapRemoteBffApiEndpoint(
                        "/scheduler",
                        "https://localhost:5022/scheduler",
                        csrf_protection_enabled)
                    .WithOptionalUserAccessToken()
                    .AllowAnonymous();

                    endpoints.MapRemoteBffApiEndpoint(
                        "/swagger",
                        "https://localhost:5022/swagger",
                        csrf_protection_enabled)
                    .AllowAnonymous();
                }

            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
