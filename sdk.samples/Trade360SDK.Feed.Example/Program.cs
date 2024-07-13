using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Trade360SDK.CustomersApi.Configuration;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ.Extensions;

namespace Trade360SDK.Feed.Example;

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
                services.Configure<CustomersApiSettings>("CustomersApiInplaySettings", hostContext.Configuration.GetSection("Trade360:CustomersApiInplay"));

                // Configure the settings for the "Inplay" feed using the "Trade360:RmqInplaySettings" section of the configuration file
                services.Configure<RmqConnectionSettings>("Inplay", hostContext.Configuration.GetSection("Trade360:RmqInplaySettings"));

                // Configure the settings for the "Prematch" feed using the "Trade360:RmqPrematchSettings" section of the configuration file
                services.Configure<RmqConnectionSettings>("Prematch", hostContext.Configuration.GetSection("Trade360:RmqPrematchSettings"));

                // Add the Trade360 RabbitMQ Feed SDK services to the service collection
                services.AddT360RmqFeedSdk(hostContext.Configuration);

                services.AddHostedService<Startup>();
            });
}