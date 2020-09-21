using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace m_clippy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(ConfigurationLocalSettings)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void ConfigurationLocalSettings(HostBuilderContext hostBuilderContext, IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.AddJsonFile("local.settings.json", true, false);
        }
    }
}
