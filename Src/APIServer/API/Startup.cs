// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

using System;
using APIServer.Configuration;
using Hangfire;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shared.Aplication.Interfaces;
using Shared.Aplication.Services;

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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>{
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });

            services.AddAuth(Configuration);

            services.AddDbContext(Configuration,Environment);

            services.AddHttpClient();

            services.AddHealthChecks();

            services.AddGraphql(Environment);

            services.AddHttpContextAccessor();

            services.AddMemoryCache();

            services.AddTelemerty(Configuration,Environment);

            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddMediatR();

            services.AddScheduler(Configuration);

            services.AddSingleton(Serilog.Log.Logger);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
             IWebHostEnvironment env,
             IServiceProvider serviceProvider,
             IServiceScopeFactory scopeFactory)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions{
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost,
            });

            app.Use(async (context, next) =>
            {
                System.Console.WriteLine("Headers");
                foreach (var item in context.Request.Headers)
                {
                    System.Console.WriteLine("{0}: {1}",item.Key, item.Value);
                }
                // Call the next delegate/middleware in the pipeline
                await next();
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
    }
}
