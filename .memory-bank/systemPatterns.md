# System Patterns

## Architecture Overview

The SDK follows a **layered architecture** with clear separation of concerns:

### Layer Structure

```
┌─────────────────────────────────────┐
│      Client Application Layer       │
│  (Customer's .NET Application)      │
└─────────────────┬───────────────────┘
                  │
┌─────────────────▼───────────────────┐
│         SDK Public API Layer        │
│  - Feed SDK (RabbitMQ)              │
│  - Snapshot API (HTTP)              │
│  - Customers API (HTTP)             │
└─────────────────┬───────────────────┘
                  │
┌─────────────────▼───────────────────┐
│      Common Entities Layer          │
│  - Message Types (37+ types)        │
│  - Converters & Serialization       │
│  - Attributes & Helpers             │
└─────────────────┬───────────────────┘
                  │
┌─────────────────▼───────────────────┐
│       Transport Layer               │
│  - RabbitMQ.Client                  │
│  - HttpClient                       │
└─────────────────────────────────────┘
```

## Key Design Patterns

### 1. Dependency Injection Pattern
**Purpose**: Enable loose coupling and testability

```csharp
services.AddTrade360Feed(config => {
    config.Username = "user";
    config.Password = "pass";
});
```

**Benefits**:
- Easy to mock dependencies in tests
- Configuration centralized in DI container
- Lifetime management handled by framework

### 2. Message Router Pattern
**Purpose**: Route incoming messages to appropriate handlers based on type

```csharp
// Messages are automatically routed to typed handlers
public class LivescoreHandler : IMessageProcessor<LivescoreUpdate>
{
    public Task ProcessAsync(LivescoreUpdate message) { }
}
```

**Implementation**:
- Each message type has `[Trade360Entity(id)]` attribute
- Router uses dictionary lookup by message type ID
- Type-safe handler registration via generics

### 3. Repository Pattern (HTTP APIs)
**Purpose**: Abstract data access behind clean interfaces

```csharp
public interface ISnapshotApiClient
{
    Task<FixtureSnapshot> GetFixtureSnapshotAsync(int fixtureId);
    Task<IEnumerable<MarketSnapshot>> GetMarketSnapshotsAsync(int fixtureId);
}
```

**Benefits**:
- Easy to mock for testing
- Can swap implementations (e.g., caching layer)
- Clear API contracts

### 4. Factory Pattern
**Purpose**: Create configured instances of complex objects

```csharp
// Connection factory creates configured RabbitMQ connections
ConnectionFactory factory = new ConnectionFactory
{
    UserName = settings.Username,
    Password = settings.Password,
    // ... other settings
};
```

### 5. Strategy Pattern (Message Processing)
**Purpose**: Different handling strategies for different message types

```csharp
// Different processors for different message types
IMessageProcessor<FixtureMetadataUpdate> fixtureProcessor;
IMessageProcessor<LivescoreUpdate> livescoreProcessor;
IMessageProcessor<MarketUpdate> marketProcessor;
```

### 6. Observer Pattern (Message Consumption)
**Purpose**: Notify subscribers of incoming messages

```csharp
// Clients register handlers, SDK notifies on message arrival
public interface IMessageProcessor<T> where T : MessageUpdate
{
    Task ProcessAsync(T message);
}
```

### 7. Circuit Breaker Pattern (Polly)
**Purpose**: Prevent cascading failures and allow recovery time

```csharp
// HTTP clients use Polly for resilience
services.AddHttpClient<ICustomersApiClient>()
    .AddPolicyHandler(GetRetryPolicy());
```

## Component Relationships

### Feed SDK Component
```
┌──────────────────────────────┐
│   FeedServiceCollection      │ ← DI Registration
│   Extensions                 │
└──────────┬───────────────────┘
           │
┌──────────▼───────────────────┐
│   MessageConsumer            │ ← RabbitMQ Consumer
│   (AsyncDefaultBasicConsumer)│
└──────────┬───────────────────┘
           │
┌──────────▼───────────────────┐
│   MessageProcessor           │ ← Message Router
│   Container                  │
└──────────┬───────────────────┘
           │
┌──────────▼───────────────────┐
│   IMessageProcessor<T>       │ ← Customer Handlers
└──────────────────────────────┘
```

### API Clients Component
```
┌──────────────────────────────┐
│   ISnapshotApiClient         │
│   ICustomersApiClient        │
└──────────┬───────────────────┘
           │
┌──────────▼───────────────────┐
│   HttpClient                 │ ← Polly Policies
│   + AutoMapper               │
└──────────┬───────────────────┘
           │
┌──────────▼───────────────────┐
│   Trade360 HTTP APIs         │
└──────────────────────────────┘
```

## Key Technical Decisions

### 1. .NET Standard 2.1
**Decision**: Target .NET Standard 2.1 for maximum compatibility
**Rationale**: 
- Supports both .NET Core 3.0+ and .NET Framework 4.8+
- Modern C# features (nullable references, async streams)
- Broad platform support (Unity, Xamarin, Mono)

### 2. System.Text.Json over Newtonsoft.Json
**Decision**: Use System.Text.Json for serialization
**Rationale**:
- Better performance (2-3x faster)
- Lower memory allocation
- Built into .NET Core 3.0+
- Better async support

### 3. Async/Await Throughout
**Decision**: All I/O operations are async
**Rationale**:
- Non-blocking I/O for high throughput
- Better resource utilization
- Scalable under load

### 4. Attribute-Based Message Type Registration
**Decision**: Use `[Trade360Entity(id)]` attributes
**Rationale**:
- Self-documenting code
- Compile-time type safety
- Easy reflection-based discovery
- No manual registration needed

### 5. RabbitMQ.Client over MassTransit/NServiceBus
**Decision**: Use RabbitMQ.Client directly
**Rationale**:
- Lower abstraction overhead
- More control over connection management
- Lighter weight dependency
- Simpler for SDK consumers

### 6. Multiple NuGet Packages
**Decision**: Separate packages per component
**Rationale**:
- Customers only include what they need
- Smaller deployment sizes
- Independent versioning possible
- Clear separation of concerns

**Packages**:
- `Trade360SDK.Feed` - RabbitMQ feed consumption
- `Trade360SDK.Feed.RabbitMQ` - RabbitMQ-specific implementation
- `Trade360SDK.SnapshotApi` - Snapshot HTTP API
- `Trade360SDK.CustomersApi` - Customers HTTP API
- `Trade360SDK.Common.Entities` - Shared entities
- `Trade360SDK.Microsoft.DependencyInjection` - DI extensions

## Error Handling Strategy

### Connection Errors
- Automatic reconnection with exponential backoff
- Max retry attempts configurable
- Logging at each retry attempt

### Message Processing Errors
- Log error with message context
- Reject message (with requeue option)
- Continue processing next messages

### API Errors
- Polly retry policies with jitter
- Circuit breaker for cascading failure prevention
- Meaningful exception wrapping

## Testing Strategy

### Unit Tests
- Mock RabbitMQ connections
- Test message deserialization
- Verify routing logic
- Test error handling paths

### Integration Tests
- Use TestContainers for RabbitMQ
- End-to-end message flow
- API client integration tests
- Connection recovery scenarios

### Code Coverage
- Target: 80%+ coverage
- Focus on critical paths
- Message routing 100% covered
- Error handling 100% covered

