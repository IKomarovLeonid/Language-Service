using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Service
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var url = args == null || args.Length == 0 || args[0] == null ? "http://localhost:8080" : args[0];

            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().UseUrls(url);
        }
    }
}
