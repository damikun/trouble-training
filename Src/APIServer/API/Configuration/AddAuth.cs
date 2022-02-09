using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                options.Authority = Configuration.GetIdentityServerUri();

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

                /*  Use this to force disable SSL validation in dev.
                options.BackchannelHttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (
                        sender,
                        certificate,
                        chain,
                        sslPolicyErrors) => { return true; }
                };
                */
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