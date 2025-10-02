# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Package Version Summary

| Package | Version | Changes |
|---------|---------|---------|
| Trade360SDK.Feed | 2.0.0 | Breaking changes to IEntityHandler interface |
| Trade360SDK.Feed.RabbitMQ | 2.1.1 | Bug fixes for message acknowledgments in high-throughput scenarios |
| Trade360SDK.Common.Entities | 1.4.0 | Extended messaging models with venue, stage, and round support |
| Trade360SDK.SnapshotApi | 1.3.0 | Added venue support to OutrightFixtureSnapshotResponse |
| Trade360SDK.CustomersApi | 1.1.0 | Added venue management API endpoints (GetVenues, GetCities, GetStates) |

---

## [Release Version 2.3.0]

### [Trade360SDK.Common.Entities - v1.4.0]

#### Added

- **Venue-Related Enums**
  - `CourtSurface` enum with values: Grass, Hard, Clay, ArtificialGrass
  - `VenueAssignment` enum with values: Home, Away, Neutral  
  - `VenueEnvironment` enum with values: Indoors, Outdoors

- **Enhanced Fixture Entities**
  - `FixtureVenue` class with comprehensive venue information:
    - Basic venue details (Id, Name, Capacity, Attendance)
    - Court surface type, environment, and assignment properties
    - Geographic information (Country, State, City) using `IdNamePair`
  - Enhanced `Fixture` entity with new properties:
    - `Venue` property of type `FixtureVenue`
    - `Stage` property of type `IdNamePair` for tournament stage information
    - `Round` property of type `IdNamePair` for tournament round information
  - Enhanced `OutrightFixture` entity with:
    - `Venue` property of type `FixtureVenue`

- **Shared Entities**
  - `IdNamePair` class for consistent ID/Name pair representation across venue-related entities

### [Trade360SDK.SnapshotApi - v1.3.0]

#### Added
- **Venue Support for Outright Fixtures**
  - Added `Venue` property of type `FixtureVenue` to `OutrightFixtureSnapshotResponse` entity
  - Enhanced outright fixture responses to include comprehensive venue information

### [Trade360SDK.CustomersApi - v1.1.0]

#### Added

- **New Metadata API Endpoints**
  - `GetVenuesAsync()` - Retrieve venue information with filtering capabilities
  - `GetCitiesAsync()` - Retrieve city information with filtering capabilities
  - `GetStatesAsync()` - Retrieve state information with filtering capabilities

- **Request DTOs and Filters**
  - `GetVenuesRequestDto` / `VenueFilterDto` - Filter venues by venue IDs, country IDs, state IDs, and city IDs
  - `GetCitiesRequestDto` / `CityFilterDto` - Filter cities by country IDs, state IDs, and city IDs
  - `GetStatesRequestDto` / `StateFilterDto` - Filter states by country IDs and state IDs

- **Response Structures**
  - `GetVenuesResponse` with `Venue` entity containing venue ID, name, and geographic information
  - `GetCitiesResponse` with `City` entity containing city ID, name, country, and state information
  - `GetStatesResponse` with `State` entity containing state ID, name, and country information

- **Enhanced Sample Application**
  - Added menu options for venues, cities, and states metadata APIs
  - Sample implementations demonstrating new API endpoints with JSON serialization

#### API Routes

- Added `/Venues/Get` endpoint to metadata API
- Added `/Cities/Get` endpoint to metadata API  
- Added `/States/Get` endpoint to metadata API

### Backward Compatibility

All changes are backward compatible. Existing code will continue to work without modification. The new venue, stage, and round properties are optional additions to existing entities.

## [Release Version 2.2.0]

### [Trade360SDK.Common.Entities - v1.3.0]

#### Added

- **Incident Confirmation Support**
  - `IncidentConfirmation` enum with values: Confirmed, TBD, Cancelled
  - Added `Confirmation` property to `CurrentIncident` entity for incident status tracking

- **Outright League Enhancements**
  - Added `EndDate` property to `OutrightLeagueFixture` entity for tournament end date tracking

### [Trade360SDK.SnapshotApi - v1.2.0]

#### Added

- Enhanced outright fixture entities to support new `EndDate` property for tournament end date tracking

### Backward Compatibility

All changes are backward compatible. The new `Confirmation` and `EndDate` properties are optional additions to existing entities.

## [Release Version 2.1.1] - Patch Release

### [Trade360SDK.Feed.RabbitMQ - v2.1.1]

#### Fixed

- Fixed an issue with the RabbitMQFeed where it did not properly handle message acknowledgments, leading to potential message loss in high-throughput scenarios

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
