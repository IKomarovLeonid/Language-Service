using API.Src;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace API
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>().UseUrls("http://localhost:8080");
        }
    }
}
