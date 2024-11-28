using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Trade360SDK.Common.Configuration;
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
                // Configure the settings for the "Inplay" feed using the "Trade360:RmqInplaySettings" section of the configuration file
                services.Configure<RmqConnectionSettings>("Inplay", hostContext.Configuration.GetSection("Trade360:RmqInplaySettings"));

                // Configure the settings for the "Prematch" feed using the "Trade360:RmqPrematchSettings" section of the configuration file
                services.Configure<RmqConnectionSettings>("Prematch", hostContext.Configuration.GetSection("Trade360:RmqPrematchSettings"));
                
                // Configure the settings for CustomersApi using the "Trade360:Trade360Settings" section of the configuration file
                services.Configure<Trade360Settings>("customerSettings", hostContext.Configuration.GetSection("Trade360Settings"));
                
                // Add the Trade360 RabbitMQ Feed SDK services to the service collection
                services.AddT360RmqFeedSdk(hostContext.Configuration);

                // Add your handlers to handle message updates
                services.AddTrade360Handlers();

                services.BuildServiceProvider();

                services.AddHostedService<SampleService>();
            });
}