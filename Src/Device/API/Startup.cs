// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Device.Configuration;
using Microsoft.OpenApi.Models;
using SharedCore.Aplication.Interfaces;
using SharedCore.Aplication.Services;
using HotChocolate.AspNetCore;

namespace Device
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

            services.AddSwaggerGen(c =>{
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddMediatR();

            services.AddHealthChecks();
            
            services.AddGraphql(Environment);

            services.AddTokenManagment();
            
            services.AddTelemerty(Configuration,Environment);

            services.AddSingleton(Serilog.Log.Logger);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }
            else {
                app.UseExceptionHandler("/Error");
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

            app.UseEndpoints(endpoints => {

                // local APIs
                endpoints.MapControllers();

                endpoints.MapGraphQL()
                .WithOptions(new GraphQLServerOptions {
                    EnableSchemaRequests = env.IsDevelopment(),
                    Tool = { Enable = env.IsDevelopment() },
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            
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
