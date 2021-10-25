// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

using System;
using Hangfire;
using APIServer.Configuration;
using HotChocolate.AspNetCore;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using SharedCore.Aplication.Services;
using SharedCore.Aplication.Interfaces;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APIServer
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
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>{
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });

            ConfigureAuth(services);

            ConfigureDBContext(services);

            services.AddHttpClient();

            services.AddHealthChecks();

            services.AddGraphql(Environment);

            services.AddHttpContextAccessor();

            services.AddMemoryCache();

            ConfigureTelemetry(services);

            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddMediatR();

            ConfigureScheduler(services);

            services.AddSingleton(Serilog.Log.Logger);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(
            IApplicationBuilder app,
             IWebHostEnvironment env,
             IServiceProvider serviceProvider,
             IServiceScopeFactory scopeFactory)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions{
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost,
            });

            app.UseEnsureApiContextCreated(serviceProvider,scopeFactory);

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }

            app.UseHealthChecks("/health");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseHangfireServer();

            if(env.IsDevelopment()){
                app.UseHangfireDashboard("/scheduler");
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization("ApiCaller");
                
                endpoints.MapGraphQL()
                .WithOptions(new GraphQLServerOptions {
                    EnableSchemaRequests = env.IsDevelopment(),
                    Tool = { Enable = env.IsDevelopment() },
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }


        public virtual void ConfigureDBContext(IServiceCollection services)
        {
            services.AddDbContext(Configuration,Environment);
        }

        public virtual void ConfigureScheduler(IServiceCollection services)
        {
            services.AddScheduler(Configuration);
        }

        public virtual void ConfigureTelemetry(IServiceCollection services)
        {
            services.AddTelemerty(Configuration,Environment);
        }

        public virtual void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuth(Configuration);
        }
    }
}
