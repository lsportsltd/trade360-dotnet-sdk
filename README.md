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



2. **Update Customer Subscription:**
   - **Code:**
     ```csharp
     using Trade360SDK.Customers;

     var customersClient = new CustomersClient("your-api-key");
     var subscriptionUpdate = new SubscriptionUpdate
     {
         CustomerId = customerId,
         SubscriptionType = "Premium"
     };
     await customersClient.UpdateSubscriptionAsync(subscriptionUpdate);
     ```


1. **Fetch Snapshot Data:**
   - **Code:**
     ```csharp
     using Trade360SDK.Snapshot;

     var snapshotClient = new SnapshotClient("your-api-key");
     var snapshotId = "67890";
     var snapshotData = await snapshotClient.GetSnapshotDataAsync(snapshotId);
     Console.WriteLine($"Snapshot Timestamp: {snapshotData.Timestamp}");
     ```

2. **Create a New Snapshot:**
   - **Code:**
     ```csharp
     using Trade360SDK.Snapshot;

     var snapshotClient = new SnapshotClient("your-api-key");
     var newSnapshotRequest = new CreateSnapshotRequest
     {
         Data = "Example data"
     };
     var newSnapshot = await snapshotClient.CreateSnapshotAsync(newSnapshotRequest);
     Console.WriteLine($"New Snapshot ID: {newSnapshot.Id}");
     ```


### Using Customers API <a name = "usage_customers_api"></a>

Describe the process of using the customers API, including example requests and responses.

## Contributing <a name = "contributing"></a>

Please read [CONTRIBUTING.md](../CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## License <a name = "license"></a>

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

Replace `[github-releases]` with the actual link to your GitHub releases page.
