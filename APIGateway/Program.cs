using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;

namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!args.Any() || args.Length < 3)
            {
                throw new Exception("Host, Port and Configuration Filename expected");
            }
            var host = args[0];
            var port = args[1];
            var configFile = args[2];
            args = args.Where(x => x != args[0] && x != args[1]).ToArray();
            CreateHostBuilder(args, host, port, configFile).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string host, string port, string configFile) =>
            Host.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true,
                            true)
                        .AddJsonFile(configFile, true, true)
                        .AddEnvironmentVariables();
                })
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseUrls($"http://{host}:{port}");
                    webBuilder.UseStartup<Startup>();
                });
    }
}