# LSports Trade360 SDK

## Table of Contents

- [About](#about)
- [Getting Started](#getting_started)
    - [Pre-requisites](#pre_requisites)
    - [Supported .NET Versions](#supported_versions)
    - [Installing](#installing)
    - [Initial Configuration](#configuration)
- [Usage Guide](#usage_guide)
  - [Connecting to Trade360 Feed](#usage_guide_feed)
  - [Using the Snapshot API](#usage_snapshot_api)
  - [Using Customers API](#usage_customers_api)
- [Contribution](#contributing)
- [License](#license)

## About <a name = "about"></a>

The Trade360 SDK is designed to simplify the integration with Trade360 services. This SDK provides a comprehensive set of tools and examples to streamline the following tasks:

- Connecting to the Trade360 feed
- Utilizing the Snapshot API
- Interacting with the Customers API

By using this SDK, developers can easily integrate and interact with Trade360's services, ensuring efficient and effective use of the available APIs.

### Key Features
- Efficiently connect and interact with the Trade360 feed, featuring automatic recovery through configuration and seamless start/stop distribution aligned with service operations.
- Utilize the Snapshot API for real-time recovery, with an easy-to-use HTTP client exposing all relevant endpoints, including comprehensive request and response handling.
- Manage customer data and subscriptions seamlessly via the Customers API, offering an intuitive HTTP client that covers all necessary endpoints for efficient data management.

## Getting Started <a name="getting_started"></a>

This section provides examples and guidance to help you start using the Trade360 SDK.

### Prerequisites <a name = "pre_requisites"></a>

Ensure you have the following installed on your machine:

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

## Supported .NET Versions <a name = "supported_versions"></a>

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

## Installing <a name = "installing"></a>

A step-by-step series of instructions to set up your development environment.

1. **Clone the repository:**

    ```bash
    git clone https://github.com/yourusername/trade360sdk-feed-example.git
    cd trade360sdk-feed-example
    ```

2. **Restore dependencies:**

    ```bash
    dotnet restore
    ```

3. **Build the project:**

    ```bash
    dotnet build
    ```

## Usage Guide <a name = "usage_guide"></a>

### Connecting to Trade360 Feed <a name = "usage_guide_feed"></a>

This is an example usage of the feed SDK, which gives you the ability to create an instance and connect to your RabbitMQ feed. You can create a handler to deal with each type of message being produced (fixture, livescore, markets, settlement) for standard sports, outright sports, and outright league sports (tournaments). Please download the repo and run the examples for more information.

#### Example Configuration (`appsettings.json`)

```json
{
  "Trade360": {
    "RmqInplaySettings": {
      "Host": "trade360-inplay-rabbitmq-host",
      "Port": "trade360-inplay-rabbitmq-port",
      "VirtualHost": "trade360-inplay-rabbitmq-virtual-host",
      "PackageId": 0,
      "Username": "your-username",
      "Password": "your-password",
      "PrefetchCount": 100,
      "AutoAck": true,
      "RequestedHeartbeatSeconds": 30,
      "NetworkRecoveryInterval": 30,
      "DispatchConsumersAsync": true,
      "AutomaticRecoveryEnabled": true
    }
  }
}
```

#### Dependency Injection Setup (Program.cs)
After setting the correct configuration, add the following to your dependency injection:
```csharp
services.AddT360RmqFeedSdk();
```

#### Implementing The Connection

Using `IFeedFactory` and creating a connection to the desired package (inplay or prematch):

```csharp
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.Example.Handlers.Inplay;

namespace Trade360SDK.Feed.Example
{
    public class Startup : IHostedService
    {
        private readonly IFeedFactory _feedFactory;
        private readonly IOptionsMonitor<RmqConnectionSettings> _settingsMonitor;
        private IFeed? _inplayFeed;

        public Startup(IFeedFactory feedFactory, IOptionsMonitor<RmqConnectionSettings> settingsMonitor)
        {
            _feedFactory = feedFactory;
            _settingsMonitor = settingsMonitor;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var inplaySettings = _settingsMonitor.Get("Inplay");
            _inplayFeed = _feedFactory.CreateFeed(inplaySettings); // Create the IFeed instance for inplay

            // Add entity handlers to the Inplay feed
            _inplayFeed.AddEntityHandler(new HeartbeatHandlerInplay());
            _inplayFeed.AddEntityHandler(new FixtureMetadataUpdateHandlerInplay());
            _inplayFeed.AddEntityHandler(new LivescoreUpdateHandlerInplay());

            await _inplayFeed.StartAsync(cancellationToken); // Start the connection

            Console.WriteLine("Click any key to stop message consumption");
            Console.ReadLine();

            if (_inplayFeed != null) await _inplayFeed.StopAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_inplayFeed != null)
            {
                await _inplayFeed.StopAsync(cancellationToken);
            }
        }
    }
}
```

As demonstrated above, we are injecting the IFeedFactory and creating the IFeed instance for inplay by providing the relevant configuration.
1. **Inject `IFeedFactory`**:
    ```csharp
    public Startup(IFeedFactory feedFactory, IOptionsMonitor<RmqConnectionSettings> settingsMonitor)
    {
        _feedFactory = feedFactory;
        _settingsMonitor = settingsMonitor;
    }
    ```

2. **Create the `IFeed` instance for inplay**:
    ```csharp
    var inplaySettings = _settingsMonitor.Get("Inplay");
    _inplayFeed = _feedFactory.CreateFeed(inplaySettings);
    ```

3. **Add handlers for each type of message**:
    ```csharp
    _inplayFeed.AddEntityHandler(new HeartbeatHandlerInplay());
    _inplayFeed.AddEntityHandler(new FixtureMetadataUpdateHandlerInplay());
    _inplayFeed.AddEntityHandler(new LivescoreUpdateHandlerInplay());
    ```

4. **Start the connection**:
    ```csharp
    await _inplayFeed.StartAsync(cancellationToken);
    ```


### Using the Snapshot API <a name = "usage_snapshot_api"></a>

This is an example usage of the Snapshot API SDK, which provides an easy way to interact with the Snapshot API for recovery purposes. The SDK offers a simplified HTTP client with request and response handling.

#### Example Configuration (`appsettings.json`)

```json
{
  "Trade360": {
    "SnapshotInplaySettings": {
      "BaseUrl": "https://stm-snapshot.lsports.eu",
      "PackageId": 0, //Insert your package id
      "Username": "your-username",
      "Password": "your-password"
    },
    "SnapshotPrematchSettings": {
      "BaseUrl": "https://stm-snapshot.lsports.eu",
      "PackageId": 0, //Insert your package id
      "Username": "your-username",
      "Password": "your-password"
    }
  }
}
```

Dependency Injection Setup (Program.cs)
After setting the correct configuration, add the following to your dependency injection:
```csharp
services.AddT360ApiClient();
```

#### Implementing The Snapshot API Client

Using `ISnapshotApiFactory` to create and use the Snapshot API client:

```csharp
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Trade360SDK.SnapshotApi.Configuration;
using Trade360SDK.SnapshotApi.Interfaces;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Microsoft.Extensions.Logging;

namespace Trade360SDK.SnapshotApi.Example
{
    public class Startup : IHostedService
    {
        private readonly ILogger<Startup> _logger;
        private readonly ISnapshotApiFactory _snapshotApiFactory;
        private readonly IOptionsMonitor<SnapshotApiSettings> _settingsMonitor;

        public Startup(ILogger<Startup> logger, ISnapshotApiFactory snapshotApiFactory, IOptionsMonitor<SnapshotApiSettings> settingsMonitor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _snapshotApiFactory = snapshotApiFactory;
            _settingsMonitor = settingsMonitor;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var snapshotInplayApiSettings = _settingsMonitor.Get("SnapshotInplaySettings");
                var snapshotPrematchApiSettings = _settingsMonitor.Get("SnapshotPrematchSettings");

                var inplaySnapshotClient = _snapshotApiFactory.CreateInplayHttpClient(snapshotInplayApiSettings);
                var prematchSnapshotClient = _snapshotApiFactory.CreatePrematchHttpClient(snapshotPrematchApiSettings);

                // Example method call: GetFixtures
                await GetFixtures(prematchSnapshotClient, cancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving data");
            }
        }

        private async Task GetFixtures(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetFixtures...");

            var request = new GetFixturesRequestDto
            {
                Sports = new List<int> { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int> { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Leagues = new List<int> { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int> { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotPrematchApiClient.GetFixtures(request, cancellationToken);
            _logger.LogInformation("GetFixtures ended with response count: {Count}", response.Count());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");
            return Task.CompletedTask;
        }
    }
}
```

As demonstrated above, we are injecting the ISnapshotApiFactory and creating the Snapshot API client instance for inplay and prematch by providing the relevant configuration.

1. **Inject `ISnapshotApiFactory`**:
    ```csharp
    public Startup(ILogger<Startup> logger, ISnapshotApiFactory snapshotApiFactory, IOptionsMonitor<SnapshotApiSettings> settingsMonitor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _snapshotApiFactory = snapshotApiFactory;
        _settingsMonitor = settingsMonitor;
    }
    ```

2. **Create the Snapshot API client instance**:
    ```csharp
    var snapshotInplayApiSettings = _settingsMonitor.Get("SnapshotInplaySettings");
    var snapshotPrematchApiSettings = _settingsMonitor.Get("SnapshotPrematchSettings");

    var inplaySnapshotClient = _snapshotApiFactory.CreateInplayHttpClient(snapshotInplayApiSettings);
    var prematchSnapshotClient = _snapshotApiFactory.CreatePrematchHttpClient(snapshotPrematchApiSettings);
    ```

3. **Add methods for snapshot operations**:
    ```csharp
    private async Task GetFixtures(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting GetFixtures...");

        var request = new GetFixturesRequestDto
        {
            Sports = new List<int> { /* List of sport IDs, e.g., 1234, 2345 */ },
            Fixtures = new List<int> { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
            Leagues = new List<int> { /* List of league IDs, e.g., 1111, 2222 */ },
            Locations = new List<int> { /* List of location IDs, e.g., 3333, 4444 */ }
        };

        var response = await snapshotPrematchApiClient.GetFixtures(request, cancellationToken);
        _logger.LogInformation("GetFixtures ended with response count: {Count}", response.Count());
    }
    ```

4. **Call methods to interact with the API**:
    ```csharp
    await GetFixtures(prematchSnapshotClient, cancellationToken);
    ```



### Using Customers API <a name = "usage_customers_api"></a>

The Customers API SDK is made up of three parts: Package Distribution, Metadata, and Subscription. It provides a simplified HTTP client with request and response handling for various operations.

- **Package Distribution**: Start, stop, and get distribution status.
- **Metadata**: Exposes endpoints to get leagues, sports, locations, markets, and translations.
- **Subscription**: Allows subscribing and unsubscribing to a fixture or by league. It also includes manual suspension actions and quota retrieval.

#### Example Configuration (`appsettings.json`)

```json
{
  "Trade360": {
    "CustomersApiInplay": {
      "BaseUrl": "https://stm-api.lsports.eu",
      "PackageId": 0, // Insert your package id
      "Username": "your-username",
      "Password": "your-password"
    },
    "CustomersApiPrematch": {
      "BaseUrl": "https://stm-api.lsports.eu",
      "PackageId": 0, // Insert your package id
      "Username": "your-username",
      "Password": "your-password"
    }
  }
}
```


Dependency Injection Setup (Program.cs)
After setting the correct configuration, add the following to your dependency injection:
```csharp
services.AddT360ApiClient();
```


#### Implementing The Customers API Client

Using `ICustomersApiFactory` to create and use the Customers API client:

```csharp
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trade360SDK.CustomersApi.Configuration;
using Trade360SDK.CustomersApi.Interfaces;

namespace Trade360SDK.CustomersApi.Examples
{
    public class Startup : IHostedService
    {
        private readonly ILogger<Startup> _logger;
        private readonly ICustomersApiFactory _customerApiFactory;
        private readonly IOptionsMonitor<CustomersApiSettings> _settingsMonitor;

        public Startup(ILogger<Startup> logger, ICustomersApiFactory customersApiFactory, IOptionsMonitor<CustomersApiSettings> settingsMonitor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _customerApiFactory = customersApiFactory;
            _settingsMonitor = settingsMonitor;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var customersApiSettings = _settingsMonitor.Get("CustomersApiInplay");

                // Initialize API Clients (Metadata, PackageDistribution, Subscription)
                var packageDistributionApiClient = _customerApiFactory.CreatePackageDistributionHttpClient(customersApiSettings);
                var metadataApiClient = _customerApiFactory.CreateMetadataHttpClient(customersApiSettings);
                var subscriptionApiClient = _customerApiFactory.CreateSubscriptionHttpClient(customersApiSettings);

                // Example method calls
                await SubscribeToFixture(subscriptionApiClient, cancellationToken);
                await GetFixtureMetadata(metadataApiClient, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving data");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");
            return Task.CompletedTask;
        }

        private async Task SubscribeToFixture(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new FixtureSubscriptionRequestDto { Fixtures = new[] { 12345 } };
            var response = await subscriptionApiClient.SubscribeByFixture(request, cancellationToken);
            Console.WriteLine($"Send subscription request to {response.Fixtures.Count} fixtures");
        }

        private async Task GetFixtureMetadata(IMetadataApiClient metadataApiClient, CancellationToken cancellationToken)
        {
            var request = new GetFixtureMetadataRequestDto { FromDate = DateTime.Now, ToDate = DateTime.Now.AddDays(2) };
            var response = await metadataApiClient.GetFixtureMetadataAsync(request, cancellationToken);
            Console.WriteLine("Fixture metadata retrieved.");
        }
    }
}
```

1. **Inject `ICustomersApiFactory`**:
    ```csharp
    public Startup(ILogger<Startup> logger, ICustomersApiFactory customersApiFactory, IOptionsMonitor<CustomersApiSettings> settingsMonitor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _customersApiFactory = customersApiFactory;
        _settingsMonitor = settingsMonitor;
    }
    ```

2. **Create the Customers API client instance**:
    ```csharp
    var customersApiSettings = _settingsMonitor.Get("CustomersApiInplay");

    var packageDistributionApiClient = _customersApiFactory.CreatePackageDistributionHttpClient(customersApiSettings);
    var metadataApiClient = _customersApiFactory.CreateMetadataHttpClient(customersApiSettings);
    var subscriptionApiClient = _customersApiFactory.CreateSubscriptionHttpClient(customersApiSettings);
    ```

3. **Add methods for various operations**:
    ```csharp
    private async Task SubscribeToFixture(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
    {
        var request = new FixtureSubscriptionRequestDto { Fixtures = new[] { 12345 } };
        var response = await subscriptionApiClient.SubscribeByFixture(request, cancellationToken);
        Console.WriteLine($"Send subscription request to {response.Fixtures.Count} fixtures");
    }

    private async Task GetFixtureMetadata(IMetadataApiClient metadataApiClient, CancellationToken cancellationToken)
    {
        var request = new GetFixtureMetadataRequestDto { FromDate = DateTime.Now, ToDate = DateTime.Now.AddDays(2) };
        var response = await metadataApiClient.GetFixtureMetadataAsync(request, cancellationToken);
        Console.WriteLine("Fixture metadata retrieved.");
    }
    ```

4. **Start and Stop the service**:
    ```csharp
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var customersApiSettings = _settingsMonitor.Get("CustomersApiInplay");

            var packageDistributionApiClient = _customerApiFactory.CreatePackageDistributionHttpClient(customersApiSettings);
            var metadataApiClient = _customerApiFactory.CreateMetadataHttpClient(customersApiSettings);
            var subscriptionApiClient = _customerApiFactory.CreateSubscriptionHttpClient(customersApiSettings);

            await SubscribeToFixture(subscriptionApiClient, cancellationToken);
            await GetFixtureMetadata(metadataApiClient, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving data");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Service is stopping.");
        return Task.CompletedTask;
    }
    ```

## Contributing <a name = "contributing"></a>

Please read [CONTRIBUTING.md](../CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## License <a name = "license"></a>

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

Replace `[github-releases]` with the actual link to your GitHub releases page.
