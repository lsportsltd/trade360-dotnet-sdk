﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Trade360SDK.Feed.RabbitMQ; // Using RabbitMQ related classes from the SDK
using Microsoft.Extensions.Options;
using Trade360SDK.Feed.Example.Handlers.Inplay;

namespace Trade360SDK.Feed.Example
{
    public class Startup : IHostedService
    {
        private readonly ILogger<Startup> _logger;
        private readonly IFeedFactory _feedFactory; // Factory to create RabbitMQ feeds
        private readonly IOptionsMonitor<RmqConnectionSettings> _settingsMonitor; // Monitors and provides access to configuration settings

        private IFeed _inplayFeed; // Inplay feed instance
        private IFeed _prematchFeed; // Prematch feed instance

        public Startup(IFeedFactory feedFactory, IOptionsMonitor<RmqConnectionSettings> settingsMonitor, ILogger<Startup> logger)
        {
            _feedFactory = feedFactory; // Initialize the feed factory
            _settingsMonitor = settingsMonitor; // Initialize the settings monitor
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Initialize the logger, throw an exception if logger is null
        }

        public async Task StartAsync(CancellationToken cancellationToken) // Method to start the service
        {
            // Get the settings for the Inplay feed - look at progrem.cs initialization
            var inplaySettings = _settingsMonitor.Get("Inplay");
            // Create the Inplay feed using the factory and settings
            _inplayFeed = _feedFactory.CreateFeed(inplaySettings);

            // Add entity handlers to the Inplay feed
            _inplayFeed.AddEntityHandler(new HeartbeatHandlerInplay());
            _inplayFeed.AddEntityHandler(new FixtureMetadataUpdateHandlerInplay());
            _inplayFeed.AddEntityHandler(new LivescoreUpdateHandlerInplay());

            // Start the Inplay feed
            await _inplayFeed.StartAsync(cancellationToken);

            //// Get the settings for the Prematch feed - look at progrem.cs initialization
            //var prematchSettings = _settingsMonitor.Get("Prematch");
            //// Create the Prematch feed using the factory and settings
            //_prematchFeed = _feedFactory.CreateFeed(prematchSettings);

            //// Add entity handlers to the Prematch feed
            //_prematchFeed.AddEntityHandler(new HeartbeatHandlerPrematch());
            //_prematchFeed.AddEntityHandler(new FixtureMetadataUpdateHandlerPrematch());
            //_prematchFeed.AddEntityHandler(new LivescoreUpdateHandlerPrematch());

            //// Start the Prematch feed
            //await _prematchFeed.StartAsync(cancellationToken);

            // Output a message to the console and wait for user input to stop the feeds
            Console.WriteLine("Click any key to stop message consumption");
            Console.ReadLine();

            // Stop the Inplay and Prematch feeds
            await _inplayFeed.StopAsync(cancellationToken);
            await _prematchFeed.StopAsync(cancellationToken);
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
