using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace APIServer.Configuration
{
    public static partial class ServiceExtension
    {

        public static IServiceCollection AddAuth(
            this IServiceCollection serviceCollection,
            IConfiguration Configuration)
        {


            serviceCollection.AddAuthentication("token")
            .AddJwtBearer("token", options =>
            {
                options.Authority = Configuration["ConnectionStrings:AuthorityServer"];
                options.MapInboundClaims = true;

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidateLifetime = true,

                    ValidTypes = new[] { "at+jwt" },

                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });

            serviceCollection.AddAuthorization(options =>
            {
                options.AddPolicy("ApiCaller", policy =>
                {
                    policy.RequireClaim("scope", "api");
                });

            });

            JwtSecurityTokenHandler.DefaultMapInboundClaims = true;

            return serviceCollection;
        }

    }
}