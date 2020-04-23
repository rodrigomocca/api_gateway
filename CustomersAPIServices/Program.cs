using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace CustomersAPIServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!args.Any())
            {
                throw new Exception("Port expected");
            }
            var host = args[0];
            var port = args[1];
            args = args.Where(x => x != args[0] && x != args[1]).ToArray();
            CreateHostBuilder(args, host, port).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string host, string port, string filename = "") =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls($"http://{host}:{port}");
                    webBuilder.UseStartup<Startup>();
                });
    }
}