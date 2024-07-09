using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Trade360SDK.CustomersApi.Configuration;
using Trade360SDK.CustomersApi.Extensions;

namespace Trade360SDK.Api.Examples;

internal class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((hostContext, services) =>
            {
                // Configure the settings for the "Inplay" feed using the "Trade360:RmqInplaySettings" section of the configuration file
                services.Configure<CustomersApiSettings>("CustomersApiInplaySettings", hostContext.Configuration.GetSection("Trade360:CustomersApiInplay"));

                services.AddT360ApiClient(hostContext.Configuration);
                services.AddHostedService<Startup>();
            });
}