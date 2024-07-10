
# Trade360SDK Feed Example

## Table of Contents

- [About](#about)
- [Getting Started](#getting_started)
- [Usage](#usage)
- [Contributing](../CONTRIBUTING.md)

## About <a name = "about"></a>

- Need to write about the aim of the sdk
- What it solves
- The structure of the project and what each project inside the solution aims for

## Getting Started <a name = "getting_started"></a>

- Feed Examples
- Customers Api Examples
- Snapshot Examples


### Prerequisites

Ensure you have the following installed on your machine:

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

:heavy_check_mark: The current stable major version series is: `1.x`

## Supported .NET Versions

This SDK targets .NET Standard 2.1 and is compatible with the following .NET implementations:

| .NET Implementation | Supported Versions         |
| ------------------- | -------------------------- |
| .NET Core           | :heavy_check_mark: 3.0, 3.1, 5.0, 6.0, 7.0  |
| .NET                | :heavy_check_mark: 5.0, 6.0, 7.0  |
| Mono                | :heavy_check_mark: 6.4 and later    |
| Xamarin.iOS         | :heavy_check_mark: 10.14 and later  |
| Xamarin.Mac         | :heavy_check_mark: 3.8 and later    |
| Xamarin.Android     | :heavy_check_mark: 8.0 and later    |
| Unity               | :heavy_check_mark: 2018.1 and later |

### Installing

A step-by-step series of instructions to set up your development environment.

1. **Clone the repository:**

    \```bash
    git clone https://github.com/yourusername/trade360sdk-feed-example.git
    cd trade360sdk-feed-example
    \```

2. **Configure the `appsettings.json` file:**

-Feed configuration examples
-Snapshot api examples
-Customers Api examples
    
### Example Usage

After starting the service, it will output a message to the console:

```plaintext
Click any key to stop message consumption
```

Press any key to stop the feeds and shut down the service gracefully.

## Usage <a name = "usage"></a>

### Understanding the Code

The `Startup` class is the main entry point for the service. It configures and manages the lifecycle of the RabbitMQ feeds.

\```csharp
using Microsoft.Extensions.Hosting; // Provides interfaces and classes for hosting applications
using Microsoft.Extensions.Logging; // Provides logging interfaces and classes
using Trade360SDK.Feed.Example.Handlers; // Using custom handlers for the example feed
using Trade360SDK.Feed.RabbitMQ; // Using RabbitMQ related classes from the SDK
using Microsoft.Extensions.Options; // Provides classes for accessing options

namespace Trade360SDK.Feed.Example
{
    public class Startup : IHostedService // Implements the IHostedService interface for hosting background services
    {
        private readonly ILogger<Startup> _logger; // Logger for logging information
        private readonly IRabbitMQFeedFactory _feedFactory; // Factory to create RabbitMQ feeds
        private readonly IOptionsMonitor<RmqConnectionSettings> _settingsMonitor; // Monitors and provides access to configuration settings

        private IRabbitMQFeed _inplayFeed; // Inplay feed instance
        private IRabbitMQFeed _prematchFeed; // Prematch feed instance

        public Startup(IRabbitMQFeedFactory feedFactory, IOptionsMonitor<RmqConnectionSettings> settingsMonitor, ILogger<Startup> logger)
        {
            _feedFactory = feedFactory; // Initialize the feed factory
            _settingsMonitor = settingsMonitor; // Initialize the settings monitor
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); // Initialize the logger, throw an exception if logger is null
        }

        public async Task StartAsync(CancellationToken cancellationToken) // Method to start the service
        {
            // Get the settings for the Inplay feed
            var inplaySettings = _settingsMonitor.Get("Inplay");
            // Create the Inplay feed using the factory and settings
            _inplayFeed = _feedFactory.CreateFeed(inplaySettings);

            // Add entity handlers to the Inplay feed
            _inplayFeed.AddEntityHandler(new HeartbeatHandler());
            _inplayFeed.AddEntityHandler(new FixtureMetadataUpdateHandler());
            _inplayFeed.AddEntityHandler(new LivescoreUpdateHandler());

            // Start the Inplay feed
            await _inplayFeed.StartAsync(cancellationToken);

            // Get the settings for the Prematch feed
            var prematchSettings = _settingsMonitor.Get("Prematch");
            // Create the Prematch feed using the factory and settings
            _prematchFeed = _feedFactory.CreateFeed(prematchSettings);

            // Add entity handlers to the Prematch feed
            _prematchFeed.AddEntityHandler(new HeartbeatHandler());
            _prematchFeed.AddEntityHandler(new FixtureMetadataUpdateHandler());
            _prematchFeed.AddEntityHandler(new LivescoreUpdateHandler());

            // Start the Prematch feed
            await _prematchFeed.StartAsync(cancellationToken);

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
\```

### Services Configuration

In the `Program.cs` or `Startup.cs` file, configure the services to use the settings and add the necessary services:

\```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Trade360SDK.Feed.RabbitMQ;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        // Configure the settings for the "Inplay" feed using the "Trade360:RmqInplaySettings" section of the configuration file
        services.Configure<RmqConnectionSettings>("Inplay", context.Configuration.GetSection("Trade360:RmqInplaySettings"));

        // Configure the settings for the "Prematch" feed using the "Trade360:RmqPrematchSettings" section of the configuration file
        services.Configure<RmqConnectionSettings>("Prematch", context.Configuration.GetSection("Trade360:RmqPrematchSettings"));

        // Add the Trade360 RabbitMQ Feed SDK services to the service collection
        services.AddSingleton<IRabbitMQFeedFactory, RabbitMQFeedFactory>();
        services.AddHostedService<Trade360SDK.Feed.Example.Startup>();
    })
    .Build();

await host.RunAsync();
\```

## Contributing

Please read [CONTRIBUTING.md](../CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
