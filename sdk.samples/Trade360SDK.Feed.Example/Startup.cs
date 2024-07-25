using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Trade360SDK.Common.Entities.OutrightSport;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.Example.Handlers.Inplay;
using Trade360SDK.Feed.Example.Handlers.Prematch;

namespace Trade360SDK.Feed.Example
{
    public class Startup : IHostedService
    {
        private readonly IFeedFactory _feedFactory; // Factory to create RabbitMQ feeds
        private readonly IOptionsMonitor<RmqConnectionSettings> _settingsMonitor; // Monitors and provides access to configuration settings

        private IFeed? _inplayFeed; // Inplay feed instance
        private IFeed? _prematchFeed; // Prematch feed instance

        public Startup(IFeedFactory feedFactory, IOptionsMonitor<RmqConnectionSettings> settingsMonitor)
        {
            _feedFactory = feedFactory; // Initialize the feed factory
            _settingsMonitor = settingsMonitor; // Initialize the settings monitor
        }

        public async Task StartAsync(CancellationToken cancellationToken) // Method to start the service
        {
            // Get the settings for the Inplay feed - look at Program.cs initialization
            //var inplaySettings = _settingsMonitor.Get("Inplay");
            //// Create the Inplay feed using the factory and settings
            //_inplayFeed = _feedFactory.CreateFeed(inplaySettings);

            //// Add entity handlers to the Inplay feed
            //_inplayFeed.AddEntityHandler(new HeartbeatHandlerInplay());
            //_inplayFeed.AddEntityHandler(new FixtureMetadataUpdateHandlerInplay());
            //_inplayFeed.AddEntityHandler(new LivescoreUpdateHandlerInplay());

            //Start Distribution Logic

            // Start the Inplay feed
            // await _inplayFeed.StartAsync(connectAtStart: true, cancellationToken);

            // Get the settings for the Prematch feed - look at program.cs initialization
            var prematchSettings = _settingsMonitor.Get("Prematch");
            // Create the Prematch feed using the factory and settings
            _prematchFeed = _feedFactory.CreateFeed(prematchSettings);

            // Add entity handlers to the Prematch feed
            _prematchFeed.AddEntityHandler(new HeartbeatHandlerPrematch());
            _prematchFeed.AddEntityHandler(new FixtureMetadataUpdateHandlerPrematch());
            _prematchFeed.AddEntityHandler(new LivescoreUpdateHandlerPrematch());
            _prematchFeed.AddEntityHandler(new OutrightLeagueUpdateHandlerPrematch());
            _prematchFeed.AddEntityHandler(new OutrightLeagueMarketsUpdateHandlerPrematch());
            _prematchFeed.AddEntityHandler(new OutrightFixtureUpdateHandlerPrematch());
            _prematchFeed.AddEntityHandler(new OutrightScoreUpdateHandlerPrematch());
            _prematchFeed.AddEntityHandler(new OutrightFixtureMarketUpdateHandlerPrematch());
            _prematchFeed.AddEntityHandler(new OutrightSettlementsUpdateHandlerPrematch());
            _prematchFeed.AddEntityHandler(new KeepAliveUpdateHandlerPrematchPrematch());
            _prematchFeed.AddEntityHandler(new FixtureMarketUpdateHandlerPrematch());
            _prematchFeed.AddEntityHandler(new SettlementUpdateHandlerPrematch());

            // Start the Prematch feed
            await _prematchFeed.StartAsync(connectAtStart:true, cancellationToken);

            // Output a message to the console and wait for user input to stop the feeds
            Console.WriteLine("Click any key to stop message consumption");
            Console.ReadLine();

            // Stop the Inplay and Prematch feeds
            if (_inplayFeed != null) await _inplayFeed.StopAsync(cancellationToken);
            if (_prematchFeed != null) await _prematchFeed.StopAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken) // Method to stop the service
        {
            // Stop the Inplay feed if it is running
            if (_inplayFeed != null)
            {
                await _inplayFeed.StopAsync(cancellationToken);
            }

            // Stop the Prematch feed if it is running
            if (_prematchFeed != null)
            {
                await _prematchFeed.StopAsync(cancellationToken);
            }
        }
    }
}
