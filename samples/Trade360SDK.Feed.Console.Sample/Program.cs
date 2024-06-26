using Trade360SDK.Feed.Console.Sample;
using Trade360SDK.Feed.Console.Sample.Handlers;
using Trade360SDK.Feed.RabbitMQ;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var username = "";
        var password = "";
        var rmqVHost = "";
        
        var packageIdValue = "";
        
        var rmqHost = "artistic-wheat-bobcat.rmq6.cloudamqp.com";
        var rmqPort = 5672;
        
        var customersApi = "https://stm-api.lsports.eu";
        
        if (username == null ||
            password == null ||
            !int.TryParse(packageIdValue, out int packageId))
        {
            return;
        }

        var prefetchCount = 100;
        var recoveryTime = TimeSpan.FromSeconds(60);
        var consoleLogger = new ConsoleLogger();
        
        using var rmqFeed = new RabbitMQFeed(
            rmqHost, 
            rmqVHost,
            rmqPort,
            username, 
            password,
            prefetchCount, 
            recoveryTime,
            packageId, 
            customersApi,
            consoleLogger);

        rmqFeed.AddEntityHandler(new HeartbeatHandler());
        rmqFeed.AddEntityHandler(new FixtureMetadataUpdateHandler());
        rmqFeed.AddEntityHandler(new LivescoreUpdateHandler());

        var cts = new CancellationTokenSource();

        await rmqFeed.StartAsync(cts.Token);

        Console.WriteLine("Click any key to stop message consumption");
        Console.ReadLine();

        await rmqFeed.StopAsync(cts.Token);
    }
}
