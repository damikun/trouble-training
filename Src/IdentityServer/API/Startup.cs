
// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

using IdentityServer.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.HttpOverrides;
using SharedCore.Configuration;

namespace IdentityServer.API
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public static IConfiguration Configuration { get; private set; }


        public Startup(IConfiguration configuration, IWebHostEnvironment enviroment)
        {
            Environment = enviroment;

            Configuration = configuration;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsConfiguration(Environment);

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddAppIdentityDbContext(Configuration, Environment);

            services.AddHealthChecks();

            services.AddMvc();

            services.AddIdentityServer(Configuration, Environment);

            services.AddTelemerty(Configuration, Environment);

            // ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) =>
            // {
            //     // local dev, just approve all certs
            //     if (Environment.IsDevelopment()) return true;

            //     return errors == SslPolicyErrors.None;
            // };
        }

        public virtual void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                IdentityServer.Configuration.ServiceExtension.InitializeDbTestData(app);
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
            });

            //app.UseElasticApm(Configuration, new IDiagnosticsSubscriber [0]);

            app.UseHealthChecks("/health");

            app.UseCookiePolicy();

            app.UseCors("cors_policy");

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 5;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{Action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}
