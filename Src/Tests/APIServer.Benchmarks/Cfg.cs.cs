using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace APIServer.Benchmark
{
    public static class Cfg
    {
        static Cfg()
        {
            var builder = new ConfigurationBuilder()
              .AddJsonFile("config.json", optional: false, reloadOnChange: true);

            config = builder.Build();

            //-------------------------------------------------

            APIServerUri = new Uri(config["Cfg:APIServerUri"]);

            IdenitiyServerUri = new Uri(config["Cfg:IdenitiyServerUri"]);

            GraphqlEndpoint = config["Cfg:GraphqlEndpoint"];

            //-------------------------------------------------

            DefaultUser = config["DefaultUser:UserName"];

            DefaulUserPassword = config["DefaultUser:Password"];

            //-------------------------------------------------

            OIDC_ClientId = config["OIDC_Client:ClinetId"];

            OIDC_ClientSecret = config["OIDC_Client:ClientSecret"];

            OIDC_ClientScope = config["OIDC_Client:Scope"];

            //-------------------------------------------------

            Config_APIServer = config["Cfg:Config_APIServer"];

            Config_IdentityServer = config["Cfg:Config_IdentityServer"];
        }

        private static IConfiguration config { get; set; }

        public static readonly Uri APIServerUri;
        public static readonly Uri IdenitiyServerUri;
        public static readonly string GraphqlEndpoint;

        //-------------------------------------------------

        public static readonly string DefaultUser;
        public static readonly string DefaulUserPassword;

        //-------------------------------------------------

        public static readonly string OIDC_ClientId;
        public static readonly string OIDC_ClientSecret;
        public static readonly string OIDC_ClientScope;

        //-------------------------------------------------

        public static readonly string Config_APIServer;
        public static readonly string Config_IdentityServer;
    }
}