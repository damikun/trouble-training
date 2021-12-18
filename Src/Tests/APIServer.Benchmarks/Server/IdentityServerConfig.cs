using Duende.IdentityServer.Models;
using IdentityModel;
using System.Collections.Generic;
using System.Security.Claims;

namespace APIServer.Benchmark
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "test",

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    AllowOfflineAccess = true,

                    AllowedScopes = { "openid", "profile", "api" }
                }
            };
        }
    }
    public class Resources
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


    public class Users
    {
        public static List<Duende.IdentityServer.Test.TestUser> Get()
        {
            return new List<Duende.IdentityServer.Test.TestUser>
            {
                new Duende.IdentityServer.Test.TestUser
                {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "testuser",
                    Password = "testuser",
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