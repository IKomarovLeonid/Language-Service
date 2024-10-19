using API.Src;
using API.Src.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.IO;

namespace API
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = ReadConfiguration();

            CreateWebHostBuilder(configuration.BaseUrl).Build().Run();
        }

        static ApplicationConfiguration ReadConfiguration()
        {
            return JsonConvert.DeserializeObject<ApplicationConfiguration>(File.ReadAllText("appsettings.json"));
        }

        private static IWebHostBuilder CreateWebHostBuilder(string url)
        {
            return WebHost.CreateDefaultBuilder().UseStartup<Startup>().UseUrls(url);
        }
    }
}
