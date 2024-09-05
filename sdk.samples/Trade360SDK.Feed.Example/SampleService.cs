using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Feed.Configuration;

namespace Trade360SDK.Feed.Example
{
    public class SampleService : IHostedService
    {
        private readonly IFeed? _inplayFeed; // Inplay feed instance
        private readonly IFeed? _prematchFeed; // Prematch feed instance

        public SampleService(IFeedFactory feedFactory, IOptionsMonitor<RmqConnectionSettings> rmqSettingsMonitor, IOptionsMonitor<Trade360Settings> customerSettingsMonitor)
        {

            // Get the settings for the Prematch or Inplay feed and customersApi - look at program.cs initialization
            var inplaySettings = rmqSettingsMonitor.Get("Inplay");
            var prematchSettings = rmqSettingsMonitor.Get("Prematch");
            var customerSetting = customerSettingsMonitor.Get("customerSettings");
            
            // Create the Prematch feed using the factory and settings
            _prematchFeed = feedFactory.CreateFeed(prematchSettings, customerSetting, FlowType.PreMatch);
            
            //// Create the Inplay feed using the factory and settings
            _inplayFeed = feedFactory.CreateFeed(inplaySettings, customerSetting, FlowType.InPlay);
        }

        public async Task StartAsync(CancellationToken cancellationToken) // Method to start the service
        {
            // Start the InPlay feed
            await _inplayFeed.StartAsync(connectAtStart:true, cancellationToken);
            
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
