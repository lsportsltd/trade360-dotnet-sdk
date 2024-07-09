using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Trade360SDK.SnapshotApi.Configuration;
using Trade360SDK.SnapshotApi.Extensions;

namespace Trade360SDK.SnapshotApi.Example;

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
                services.Configure<SnapshotApiSettings>("SnapshotInplaySettings", hostContext.Configuration.GetSection("Trade360:SnapshotInplaySettings"));
                services.Configure<SnapshotApiSettings>("SnapshotPrematchSettings", hostContext.Configuration.GetSection("Trade360:SnapshotPrematchSettings"));

                services.AddT360ApiClient(hostContext.Configuration);
                services.AddHostedService<Startup>();
            });
}