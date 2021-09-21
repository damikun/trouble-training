// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Duende.Bff;
using System.Collections.Generic;
using Microsoft.AspNetCore.HttpOverrides;
using BFF.Configuration;
using Duende.Bff.EntityFramework;
using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using System.Diagnostics;
using BFF.Domain;
using System;
using System.Net.Http;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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

            
            services.AddTelemerty(Configuration,Environment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseElasticApm(Configuration, new IDiagnosticsSubscriber [0]);

            app.UseHealthChecks("/health");

            app.UseHttpsRedirection();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseSpaStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseBff();

            app.UseAuthorization(); // adds authorization for local and remote API endpoints

            app.UseEndpoints(endpoints => {
                // local APIs
                endpoints.MapControllers()
                    .RequireAuthorization()
                    .AsBffApiEndpoint();
                    
                endpoints.MapBffManagementEndpoints();   // login, logout, user, backchannel logout...

                bool csrf_protection_enabled =  !env.IsDevelopment();
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

                if (env.IsDevelopment()) {

                    endpoints.MapRemoteBffApiEndpoint(
                        "/hookloopback",
                        "https://localhost:5022/api/Hook/hookloopback",
                        false)    
                    .AllowAnonymous();

                    endpoints.MapRemoteBffApiEndpoint(
                        "/scheduler",
                        "https://localhost:5022/scheduler",
                        csrf_protection_enabled)
                    .WithOptionalUserAccessToken()  
                    .AllowAnonymous();
                }
            
            });

            app.UseSpa(spa => {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment()){
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
