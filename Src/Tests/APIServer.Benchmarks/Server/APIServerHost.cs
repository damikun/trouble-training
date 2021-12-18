using System;
using Microsoft.Extensions.Hosting;

namespace APIServer.Benchmark
{
    public class APIServerHost : ServerBase
    {
        public APIServerHost(Uri? host_url) : base(host_url)
        {

        }

        public override IHost ConfigureServer()
        {
            return DefaultHostBuilder<APIServerStartup>(
                new string[0], Cfg.Config_APIServer, this.URL
            ).Build();
        }
    }
}