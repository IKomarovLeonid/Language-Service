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
            string settingsFile = "appsettings.json";

            if (!File.Exists(settingsFile))
            {
                var data = new ApplicationConfiguration();
                File.WriteAllText(settingsFile, JsonConvert.SerializeObject(data));
                return data;
            }
            else
            {
                return JsonConvert.DeserializeObject<ApplicationConfiguration>(File.ReadAllText(settingsFile));
            }
        }

        private static IWebHostBuilder CreateWebHostBuilder(string url)
        {
            return WebHost.CreateDefaultBuilder().UseStartup<Startup>().UseUrls(url);
        }
    }
}
