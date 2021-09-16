using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer.API
{
    internal class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "oauthClient",
                    ClientName = "Example client application using client credentials",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {new Secret("SuperSecretPassword".Sha256())}, // change me!
                    AllowedScopes = new List<string> {"api"}
                },
                new Client
                {
                    ClientId = "spa",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,

                    RedirectUris = { "https://localhost:5015/signin-oidc" },
                    
                    //FrontChannelLogoutUri = "https://localhost:5002/signout-oidc",
                    BackChannelLogoutUri = "https://localhost:5015/bff/backchannel",
                    
                    PostLogoutRedirectUris = { "https://localhost:5015/signout-callback-oidc" },

                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:3000", "http://localhost:5001",
                    },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "api" }
                }
            };
        }
    }
 internal class Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = "api",
                    DisplayName = "API #1",
                    Description = "Allow the application to access API",
                    Scopes = new List<string> {"api.read", "api.write"},
                    ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())}, // change me!
                    UserClaims = new List<string> {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Role,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.ClientId,
                        JwtClaimTypes.SessionId
                        }
                    
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new ApiScope[]
            {
                new ApiScope("api", new[] { 
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Role,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.ClientId,
                    JwtClaimTypes.SessionId
                    }),
            };
        }
    }


    internal class Users
    {
        public static List<TestUser> Get()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "testuser",
                    Password = "testuser", //TroubleUser123!
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Email, "dalo@trouble.com"),
                        new Claim(JwtClaimTypes.Role, "admin"),
                        new Claim(JwtClaimTypes.FamilyName, "Trouble"),
                        new Claim(JwtClaimTypes.ClientId, "5BE86359-073C-434B-AD2D-A3932222DABE")
                    }
                }
            };
        }
    }
}