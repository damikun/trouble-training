using System;
using APIServer.Persistence;
using APIServer.Configuration;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SharedCore.Aplication.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using SharedCore.Aplication.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace APIServer.Benchmark
{
    public class APIServerStartup : APIServer.Startup
    {
        public APIServerStartup(
            IWebHostEnvironment environment,
            IConfiguration configuration)
            : base(configuration, environment)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHttpContextAccessor();

            services.AddPooledDbContextFactory<ApiDbContext>(
                (s, o) => o
                    // .UseSqlite("DataSource=:memory:"));
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services.AddHealthChecks();

            services.AddMapper();

            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddGraphql(Environment);

            services.AddTelemetryService(Configuration, out string source);

            services.AddMediatR();

            services.AddAuthentication("token")
            .AddJwtBearer("token", options =>
            {
                options.Authority = "https://localhost:5001";

                options.MapInboundClaims = true;

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidTypes = new[] { "at+jwt" },

                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiCaller", policy =>
                {
                    policy.RequireClaim("scope", "api");
                });
            });

            JwtSecurityTokenHandler.DefaultMapInboundClaims = true;

            services.AddSingleton(Serilog.Log.Logger);
        }

        public override void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider,
            IServiceScopeFactory scopeFactory)
        {

            app.UseForwardedHeaders();

            app.UseHealthChecks("/health");

            app.UseEnsureApiContextCreated(serviceProvider, scopeFactory);

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization("ApiCaller");

                endpoints.MapGraphQL()
                .WithOptions(new GraphQLServerOptions
                {
                    EnableSchemaRequests = env.IsDevelopment(),
                    Tool = { Enable = false },
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

        }
    }
}