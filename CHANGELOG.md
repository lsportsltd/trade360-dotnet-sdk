# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Package Version Summary

| Package | Version | Changes |
|---------|---------|---------|
| Trade360SDK.Feed | 2.0.0 | Breaking changes to IEntityHandler interface |
| Trade360SDK.Feed.RabbitMQ | 2.1.0 | Breaking changes (2.0.0) + OutrightLeagueSettlementUpdate support (2.1.0) |
| Trade360SDK.Common.Entities | 1.2.0 | New outright league entities and message types |
| Trade360SDK.SnapshotApi | 1.1.0 | New GetOutrightLeagueEvents API method |
| Trade360SDK.CustomersApi | 1.0.1 | No changes in this release |

---

## [Release Version 2.1.0]

### [Trade360SDK.Feed.RabbitMQ - v2.1.0]

#### Added
- Added message processor registration for `OutrightLeagueSettlementUpdate` to support new outright league settlement messages

### [Trade360SDK.Common.Entities - v1.2.0]

#### Added
- Enhanced outright league support with new entities and message types:
  - `OutrightLeagueSettlementUpdate` message type `[Trade360Entity(43)]` for settlement updates
  - `OutrightLeagueMarket` entity with Id, Name, and Bets properties
  - Added `Markets` property to `OutrightLeagueEvent` entity to support market data

### [Trade360SDK.SnapshotApi - v1.1.0]

#### Added
- New API method for outright league events:
  - `GetOutrightLeagueEvents` method in `ISnapshotPrematchApiClient` interface
  - `GetOutrightLeagueEventsResponse` entity for API responses
  - Support for retrieving outright league events with competition wrapper structure

### [Trade360SDK.Feed - v2.0.0]

#### Added
- New `TransportMessageHeaders` class - Extracts and provides access to RabbitMQ message headers including:
  - `MessageGuid` - Unique identifier for the message
  - `MessageType` - Type of the message being processed  
  - `TimestampInMs` - Message timestamp in milliseconds
  - `MessageSequence` - Sequence number for message ordering (optional)
  - `FixtureId` - Associated fixture identifier (optional)
- Factory method `CreateFromProperties()` for creating `TransportMessageHeaders` from RabbitMQ message properties
- Comprehensive validation for required message header properties
- Support for both string and byte array header values with UTF-8 encoding

#### Changed
- **BREAKING CHANGE**: `IEntityHandler<TType, TFlow>.ProcessAsync()` method signature
  - **Before**: `Task ProcessAsync(MessageHeader? header, TType? entity)`
  - **After**: `Task ProcessAsync(TransportMessageHeaders? transportMessageHeaders, MessageHeader? header, TType? entity)`

### [Trade360SDK.Feed.RabbitMQ - v2.0.0]

#### Changed
- **BREAKING CHANGE**: `IMessageProcessor.ProcessAsync()` method signature  
  - **Before**: `Task ProcessAsync(Type type, MessageHeader? header, string? body)`
  - **After**: `Task ProcessAsync(Type type, TransportMessageHeaders? transportMessageHeaders, MessageHeader? header, string? body)`
- Updated `MessageConsumer` to extract RabbitMQ message headers and pass them through the processing pipeline
- Updated `MessageProcessor` implementation to handle the new `TransportMessageHeaders` parameter

### [Trade360SDK.Common.Entities - v1.1.0]

#### Changed
- **BREAKING CHANGE**: Removed `MessageBrokerTimestamp` property from `MessageHeader` class
  - This timestamp information is now available via `TransportMessageHeaders.TimestampInMs`

#### Fixed
- Improved message processing pipeline to preserve transport-level metadata from RabbitMQ



### Migration Guide

#### For Custom Entity Handlers

If you have implemented custom `IEntityHandler<TType, TFlow>` classes, you need to update the `ProcessAsync` method signature:

```csharp
// Before
public class MyHandler : IEntityHandler<MyEntity, InPlay>
{
    public Task ProcessAsync(MessageHeader? header, MyEntity? entity)
    {
        // Your processing logic
        return Task.CompletedTask;
    }
}

// After  
public class MyHandler : IEntityHandler<MyEntity, InPlay>
{
    public Task ProcessAsync(TransportMessageHeaders? transportMessageHeaders, MessageHeader? header, MyEntity? entity)
    {
        // Access transport headers
        if (transportMessageHeaders != null)
        {
            var messageGuid = transportMessageHeaders.MessageGuid;
            var timestamp = transportMessageHeaders.TimestampInMs; // Replaces header.MessageBrokerTimestamp
            // Use transport metadata as needed
        }
        
        // Your existing processing logic
        return Task.CompletedTask;
    }
}
```

#### For Custom Message Processors

If you have implemented custom `IMessageProcessor` classes, update the `ProcessAsync` method signature:

```csharp
// Before
public Task ProcessAsync(Type type, MessageHeader? header, string? body)
{
    // Your processing logic
    return Task.CompletedTask;
}

// After
public Task ProcessAsync(Type type, TransportMessageHeaders? transportMessageHeaders, MessageHeader? header, string? body)
{
    // Access transport headers
    if (transportMessageHeaders != null)
    {
        var messageType = transportMessageHeaders.MessageType;
        var sequence = transportMessageHeaders.MessageSequence;
        // Use transport metadata as needed
    }
    
    // Your existing processing logic  
    return Task.CompletedTask;
}
```

#### MessageBrokerTimestamp Migration

If you were previously using `MessageHeader.MessageBrokerTimestamp`, replace it with `TransportMessageHeaders.TimestampInMs`:

```csharp
// Before
public Task ProcessAsync(MessageHeader? header, MyEntity? entity)
{
    if (header?.MessageBrokerTimestamp != null)
    {
        var timestamp = header.MessageBrokerTimestamp.Value;
        // Process timestamp
    }
}

// After
public Task ProcessAsync(TransportMessageHeaders? transportMessageHeaders, MessageHeader? header, MyEntity? entity)
{
    if (transportMessageHeaders?.TimestampInMs != null)
    {
        var timestamp = transportMessageHeaders.TimestampInMs;
        // Process timestamp (now as string in milliseconds)
    }
}
```

#### Benefits of This Change

- **Enhanced Observability**: Access to message-level metadata for logging, debugging, and monitoring
- **Message Ordering**: Support for message sequence tracking
- **Correlation**: Message GUID for end-to-end request correlation
- **Temporal Analysis**: Precise message timestamps for performance analysis
- **Fixture Context**: Direct access to fixture IDs where applicable

---

## [v1.0.0] - Initial Release


### Added (v1.0.0)

- Initial SDK implementation with support for:
  - Feed consumption via RabbitMQ
  - Snapshot API client
  - Customers API client
  - Comprehensive entity models
  - Dependency injection extensions
  - Sample applications and handlers

[v1.0.0]: https://github.com/trade360/trade360-dotnet-sdk/releases/tag/v1.0.0
