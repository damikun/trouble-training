
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace APIServer.Configuration {
    public static partial class ServiceExtension {

        public static IServiceCollection AddAuth(
            this IServiceCollection serviceCollection,
            IConfiguration Configuration) {


            serviceCollection.AddAuthentication("token")
                .AddJwtBearer("token", options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.MapInboundClaims = false;

                    

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
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

            return serviceCollection;
        }

    }
}