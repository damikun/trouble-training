using System.Diagnostics;
using System.Reflection;

namespace IdentityServer.Domain {

      public static class Sources {

        // Implemented based on https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Api/README.md#instrumenting-a-libraryapplication-with-net-activity-api 
        private static readonly AssemblyName AssemblyName
        = typeof(Sources).Assembly.GetName();
        internal static readonly ActivitySource ActivitySource
            = new(AssemblyName.Name, AssemblyName.Version.ToString());

        public static readonly ActivitySource DemoSource = new("identityserver");

    }

}