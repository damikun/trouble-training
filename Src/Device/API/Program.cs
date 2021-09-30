// Copyright (c) Dalibor Kundrat All rights reserved.
// See LICENSE in root.

using System;
using Serilog;
using Device.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Device
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                ServiceExtension.ConfigureLogging(host);

                host.Run();
            }
            catch (Exception ex){
                Log.Fatal(ex, "Runtime unhandled exception");
            }
            finally{
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}
