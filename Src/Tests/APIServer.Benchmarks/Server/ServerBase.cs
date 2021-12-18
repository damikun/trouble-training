using System;
using Nuke.Common;
using Nuke.Common.IO;
using System.Net.Http;
using System.Threading;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace APIServer.Benchmark
{
    public abstract class ServerBase
    {
        private IHost? Server { get; set; } = null;

        public Uri? URL { get; private set; } = null;

        protected ServerBase(Uri? server_url)
        {
            this.URL = server_url;

            this.CreateServer();
        }

        public void CreateServer()
        {
            Server = ConfigureServer();
        }

        public abstract IHost ConfigureServer();

        public async Task StartServerAsync(CancellationToken ct)
        {
            await ValidateServer().StartAsync(ct);
        }

        public void StartServer()
        {
            ValidateServer().Start();
        }

        public async Task StopServerAsync(CancellationToken ct)
        {
            await ValidateServer().StopAsync(ct);
        }

        private IHost ValidateServer()
        {
            if (Server is null)
                throw new Exception($"{this.GetType().Name} is not cofigured!");

            return Server;
        }

        public static IHostBuilder DefaultHostBuilder<T>(
            string[] args,
            string app_settings_path,
            Uri? server_url = null) where T : class =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((ctx, cfg) =>
                    {
                        cfg.AddJsonFile(app_settings_path, optional: false, reloadOnChange: true);
                    })
                    .UseStartup<T>();

                    webBuilder.ConfigureLogging(logging =>
                    {
                        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Error);
                    });

                    if (server_url is not null)
                    {
                        webBuilder.UseUrls(server_url.AbsoluteUri);
                    }
                });

        public async Task WaitForHost(string health_endpoint = "health")
        {
            if (URL == null)
            {
                return;
            }
            else
            {
                Log($"Waiting for: {this.GetType().Name} to be on {this.URL.ToString()}");
            }

            var handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = delegate { return true; },
            };

            var client = new HttpClient(handler);

            int retry = 15;

            for (int i = 1; i <= retry; i++)
            {
                await Task.Delay(500 * retry);

                try
                {
                    var response = await client.GetAsync(URL.ToString() + health_endpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        Log($"{this.GetType().Name} is available!");
                        return;
                    }
                    else
                    {

                        continue;
                    }
                }
                catch { }

            }

            throw new System.Exception($"{this.GetType().Name} is not available");
        }

        private static void Log(string message)
        {
            System.Console.WriteLine(message);
        }

        public static AbsolutePath GetRootDirectory()
        {
            string methodName = "GetRootDirectory";

            Type type = typeof(NukeBuild);

            MethodInfo info = type.GetMethod(
                methodName,
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy
            );

#nullable enable
            return (AbsolutePath)info?.Invoke(null, null);
#nullable disable
        }
    }
}