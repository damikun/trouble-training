using System;
using Microsoft.Extensions.Hosting;

namespace APIServer.Benchmark
{
    public class IdentityServerHost : ServerBase
    {
        public IdentityServerHost(Uri? host_url) : base(host_url)
        {

        }

        public override IHost ConfigureServer()
        {
            return DefaultHostBuilder<IdentityServerStartup>(
                new string[0], Cfg.Config_IdentityServer, this.URL
            ).Build();
        }
    }
}