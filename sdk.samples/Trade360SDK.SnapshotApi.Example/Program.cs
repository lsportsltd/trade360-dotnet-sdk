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
                var inplaySettings = new SnapshotApiSettings();
                hostContext.Configuration.GetSection("Trade360:SnapshotInplaySettings").Bind(inplaySettings);

                var prematchSettings = new SnapshotApiSettings();
                hostContext.Configuration.GetSection("Trade360:SnapshotPrematchSettings").Bind(prematchSettings);

                services.AddTrade360InplaySnapshotClient(inplaySettings);
                services.AddTrade360PrematchSnapshotClient(prematchSettings);
                services.AddHostedService<Startup>();
            });
}