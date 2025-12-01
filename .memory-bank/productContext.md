# Product Context

## Why This Project Exists

Trade360 is LSports' premium sports data distribution platform. Customers need a reliable, easy-to-use way to integrate this data into their .NET applications. Without a dedicated SDK, integration would require:

- Manual RabbitMQ connection management
- Custom message deserialization logic
- Complex error handling and recovery
- Direct HTTP API calls with no type safety
- Subscription management complexity

## Problems It Solves

### 1. Integration Complexity
- **Problem**: RabbitMQ setup and message handling is complex
- **Solution**: Pre-configured consumers with automatic routing

### 2. Data Consistency
- **Problem**: Connection drops can cause missed messages
- **Solution**: Automatic snapshot recovery fills gaps

### 3. Type Safety
- **Problem**: JSON messages require manual parsing and validation
- **Solution**: Strongly-typed entities for all message types (37+ types)

### 4. Connection Management
- **Problem**: Network issues require manual reconnection logic
- **Solution**: Built-in automatic recovery with exponential backoff

### 5. Message Routing
- **Problem**: Different message types need different handlers
- **Solution**: Type-based routing with dependency injection

## How It Should Work

### Real-time Data Flow
1. Customer application registers message handlers for desired data types
2. SDK connects to RabbitMQ with customer credentials
3. Messages arrive and are automatically deserialized to typed entities
4. SDK routes messages to appropriate handlers based on type
5. On connection loss, SDK recovers and fetches missed data via Snapshot API

### API Operations Flow
1. Customer uses typed API clients (Customers, Snapshot)
2. SDK handles authentication, request/response serialization
3. Strongly-typed responses are returned to application
4. Errors are wrapped in meaningful exceptions

## User Experience Goals

### For Developers
- **5-minute quickstart**: From zero to receiving messages
- **IntelliSense everywhere**: Full type information in IDE
- **Minimal configuration**: Sensible defaults, override when needed
- **Clear errors**: Meaningful exception messages
- **Observable**: Built-in logging integration

### For Operations
- **Health monitoring**: Built-in heartbeat detection
- **Automatic recovery**: No manual intervention needed
- **Configurable retry**: Control backoff and retry policies
- **Production-ready**: Battle-tested at scale

## Message Types Supported

| Type | Name | Description |
|------|------|-------------|
| 1 | Fixture Metadata | Match schedules, participants, status |
| 2 | Livescore | Real-time scores, incidents, statistics |
| 3 | Market Updates | Betting markets and odds |
| 31 | Keep Alive | Connection health checks |
| 32 | Heartbeat | System heartbeat |
| 35 | Settlements | Market results |
| 37 | Outright Fixture | Outright competition fixtures |
| 38 | Outright League | League-level outright data |
| 39 | Outright Score | Outright competition scores |
| 40 | Outright League Market | League market updates |
| 41 | Outright Fixture Market | Fixture market updates |
| 42 | Outright Settlements | Outright settlements |
| 43 | Outright League Settlement | League settlements |

## Key Scenarios

1. **Live Betting Platform**: Receive market updates in real-time to display odds
2. **Livescore Application**: Show live match scores and incidents
3. **Data Analytics**: Collect and analyze sports data at scale
4. **Subscription Management**: Control which fixtures are delivered
5. **Recovery After Downtime**: Catch up on missed data automatically

