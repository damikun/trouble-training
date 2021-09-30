using Microsoft.Extensions.DependencyInjection;
using IdentityModel.Client;
using System;

namespace Device.Configuration {
    public static partial class ServiceExtension {

        private const string BaseAPIAddress = "https://localhost:5022/api/";

        public static IServiceCollection AddTokenManagment(this IServiceCollection services) {


            services.AddClientAccessTokenManagement(options =>
            {
                options.Clients.Add("identityserver", new ClientCredentialsTokenRequest
                {
                    Address = "https://localhost:5001/connect/token",
                    ClientId = "device",
                    ClientSecret = "secret",
                    Scope = "api"
                });

            });
            //.ConfigureBackchannelHttpClient()
            // .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[]
            // {
            //     TimeSpan.FromSeconds(1),
            //     TimeSpan.FromSeconds(2),
            //     TimeSpan.FromSeconds(3)
            // }));

            services.AddClientAccessTokenHttpClient("test_auth_client", configureClient: client =>
            {
                client.BaseAddress = new Uri(BaseAPIAddress);
            });

            services.AddHttpClient("client_without_token_managment",opt=>opt.BaseAddress = new Uri(BaseAPIAddress));

            return services;
        }
    }
}