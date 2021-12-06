using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace SharedCore.Aplication.Extensions
{

    public static partial class Extensions
    {
        public enum Protocol
        {
            http,
            https,
        }

        public const string http_regex = @"(http)?:\/\/(\S+)";
        public const string https_regex = @"(https)?:\/\/(\S+)";

        public static string GetAplicationUrl(this IServiceProvider services, Protocol protocol = Protocol.https)
        {
            var urls = services?.GetApplicationUrls();

            if (urls == null)
            {
                return null;
            }

            var regex = new Regex(protocol == Protocol.http ? http_regex : https_regex);

            return urls.Where(e => regex.IsMatch(e))?.First();
        }

        public static ICollection<string> GetApplicationUrls(this IServiceProvider services)
        {
            var server = services.GetService<IServer>();

            var addresses = server?.Features.Get<IServerAddressesFeature>();

            return addresses?.Addresses ?? Array.Empty<string>();
        }
    }
}