using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;

namespace Trade360SDK.CustomersApi.Example;

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
                services.Configure<Trade360Settings>(hostContext.Configuration.GetSection("Trade360"));

                services.AddTrade360CustomerApiClient(hostContext.Configuration);
                services.AddHostedService<SampleService>();
            });
}