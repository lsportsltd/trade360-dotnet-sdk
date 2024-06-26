using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Trade360SDK.Feed.Console.Sample;
using Trade360SDK.Feed.Example.Configuration;
using Trade360SDK.Feed.Example.Handlers;
using Trade360SDK.Feed.RabbitMQ;


namespace Trade360SDK.Feed.Example
{
    public class Startup : IHostedService
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var lsportsConfiguration = new LsportsT360Config();
            _configuration.GetSection("Trade360").Bind(lsportsConfiguration);

            var inplayRmqConfig = lsportsConfiguration.InPlayRmqConnection;
            var prematchRmqConfig = lsportsConfiguration.PreMatchRmqConnection;
            var accountDetails = lsportsConfiguration.Account;
            var customersApiUrl = lsportsConfiguration.CustomersApiUrl;

            var prefetchCount = 100;
            var recoveryTime = TimeSpan.FromSeconds(60);
            var consoleLogger = new ConsoleLogger();

            using var rmqFeed = new RabbitMQFeed(
                inplayRmqConfig.RabbitmqHost,
                inplayRmqConfig.RmqVHost,
                inplayRmqConfig.RmqPort,
                accountDetails.UserName,
                accountDetails.Password,
                prefetchCount,
                recoveryTime,
                accountDetails.InPlayPackageId,
                lsportsConfiguration.CustomersApiUrl,
                consoleLogger);

            rmqFeed.AddEntityHandler(new HeartbeatHandler());
            rmqFeed.AddEntityHandler(new FixtureMetadataUpdateHandler());
            rmqFeed.AddEntityHandler(new LivescoreUpdateHandler());

            var cts = new CancellationTokenSource();

            await rmqFeed.StartAsync(cts.Token);

            System.Console.WriteLine("Click any key to stop message consumption");
            System.Console.ReadLine();

            await rmqFeed.StopAsync(cts.Token);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
