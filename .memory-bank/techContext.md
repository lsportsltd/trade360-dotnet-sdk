# Tech Context

## Technology Stack

### Core Technologies

| Technology | Version | Purpose |
|------------|---------|---------|
| **.NET Standard** | 2.1 | Cross-platform compatibility |
| **C#** | 8.0+ | Primary programming language |
| **RabbitMQ.Client** | 6.8.1 | Message broker connectivity |
| **System.Text.Json** | 9.0.0 | JSON serialization/deserialization |
| **AutoMapper** | 12.0.1 | Object-to-object mapping |
| **Polly** | 8.4.2 | Resilience and transient fault handling |
| **xUnit** | 2.9.2 | Unit testing framework |
| **FluentAssertions** | 6.12.1 | Test assertions |
| **Moq** | 4.20.72 | Mocking framework |

### Infrastructure Components

- **Message Broker**: RabbitMQ for real-time data streaming
- **HTTP Protocol**: RESTful APIs for snapshot and metadata operations
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection
- **Configuration**: Microsoft.Extensions.Configuration
- **Logging**: Microsoft.Extensions.Logging

### Supported .NET Implementations

| Platform | Supported Versions |
|----------|-------------------|
| **.NET Core** | 3.0, 3.1, 5.0, 6.0, 7.0, 8.0+ |
| **.NET** | 5.0, 6.0, 7.0, 8.0+ |
| **Mono** | 6.4+ |
| **Xamarin.iOS** | 10.14+ |
| **Xamarin.Mac** | 3.8+ |
| **Xamarin.Android** | 8.0+ |
| **Unity** | 2018.1+ |

## Development Setup

### Prerequisites

1. **.NET SDK 8.0+**: [Download](https://dotnet.microsoft.com/download)
2. **Docker Desktop**: For running RabbitMQ locally
3. **IDE**: Visual Studio 2022, Rider, or VS Code
4. **Git**: Version control

### Local Development Environment

#### 1. Clone Repository
```bash
git clone https://github.com/lsports/trade360-dotnet-sdk.git
cd trade360-dotnet-sdk
```

#### 2. Restore Dependencies
```bash
dotnet restore
```

#### 3. Build Solution
```bash
dotnet build
```

#### 4. Run Tests
```bash
dotnet test
```

#### 5. Start Local RabbitMQ (Optional)
```bash
docker run -d --name rabbitmq \
  -p 5672:5672 \
  -p 15672:15672 \
  rabbitmq:3-management
```

### Project Structure

```
trade360-dotnet-sdk/
├── src/
│   ├── Trade360SDK.Common.Entities/       # Shared entities
│   ├── Trade360SDK.Feed/                  # Feed SDK core
│   ├── Trade360SDK.Feed.RabbitMQ/        # RabbitMQ implementation
│   ├── Trade360SDK.SnapshotApi/          # Snapshot API client
│   ├── Trade360SDK.CustomersApi/         # Customers API client
│   └── Trade360SDK.Microsoft.DependencyInjection/  # DI extensions
├── test/
│   ├── Trade360SDK.Common.Entities.Tests/
│   ├── Trade360SDK.Feed.Tests/
│   ├── Trade360SDK.Feed.RabbitMQ.Tests/
│   ├── Trade360SDK.SnapshotApi.Tests/
│   ├── Trade360SDK.CustomersApi.Tests/
│   └── Trade360SDK.Microsoft.DependencyInjection.Tests/
├── sdk.samples/
│   ├── Trade360SDK.Feed.Example/
│   ├── Trade360SDK.SnapshotApi.Example/
│   └── Trade360SDK.CustomersApi.Example/
└── trade360-dotnet-sdk.sln
```

## Technical Constraints

### Performance Requirements
- **Throughput**: Must handle 10,000+ messages per second
- **Latency**: Message processing < 10ms (95th percentile)
- **Memory**: < 500MB under normal load
- **CPU**: < 30% utilization on 2-core system

### Compatibility Constraints
- Must work with .NET Standard 2.1+
- Must support RabbitMQ 3.8+
- Must work behind corporate proxies
- Must support custom SSL certificates

### Security Constraints
- TLS/SSL required for production connections
- Credentials never logged
- API keys stored securely
- Support for mutual TLS (mTLS)

### Operational Constraints
- Zero-downtime reconnection
- Automatic recovery within 30 seconds
- Support for horizontal scaling
- Cloud-native deployment ready

## Dependencies

### Runtime Dependencies

```xml
<PackageReference Include="RabbitMQ.Client" Version="6.8.1" />
<PackageReference Include="System.Text.Json" Version="9.0.0" />
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="9.0.0" />
<PackageReference Include="Microsoft.Extensions.Logging" Version="9.0.0" />
<PackageReference Include="Microsoft.Extensions.Configuration" Version="9.0.0" />
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="Polly" Version="8.4.2" />
<PackageReference Include="Polly.Extensions.Http" Version="3.0.0" />
```

### Development Dependencies

```xml
<PackageReference Include="xunit" Version="2.9.2" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
<PackageReference Include="FluentAssertions" Version="6.12.1" />
<PackageReference Include="Moq" Version="4.20.72" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.1" />
<PackageReference Include="coverlet.collector" Version="6.0.2" />
```

## Build & CI/CD

### Build Configuration

- **Debug**: Development builds with full symbols
- **Release**: Optimized builds for NuGet packages

### CI Pipeline (GitHub Actions / Azure DevOps)

1. **Build Stage**
   - Restore dependencies
   - Build all projects
   - Run static code analysis

2. **Test Stage**
   - Run unit tests
   - Run integration tests
   - Generate code coverage reports

3. **Package Stage**
   - Create NuGet packages
   - Sign assemblies
   - Validate package metadata

4. **Publish Stage** (on tag)
   - Push to NuGet.org
   - Create GitHub release
   - Update documentation

### Code Quality Tools

- **Analyzers**: Microsoft.CodeAnalysis.NetAnalyzers
- **Code Coverage**: Coverlet
- **Code Style**: EditorConfig
- **Linting**: StyleCop, Roslyn analyzers

## Environment Configuration

### Required Environment Variables

```bash
# RabbitMQ Connection
TRADE360_RABBITMQ_HOST=rabbitmq.trade360.com
TRADE360_RABBITMQ_PORT=5672
TRADE360_RABBITMQ_USERNAME=your_username
TRADE360_RABBITMQ_PASSWORD=your_password
TRADE360_RABBITMQ_VHOST=/

# API Configuration
TRADE360_SNAPSHOT_API_URL=https://api.trade360.com/snapshot
TRADE360_CUSTOMERS_API_URL=https://api.trade360.com/customers
TRADE360_API_KEY=your_api_key

# Optional Settings
TRADE360_ENABLE_SSL=true
TRADE360_LOG_LEVEL=Information
TRADE360_RETRY_COUNT=3
```

### Configuration Sources (Priority Order)

1. Environment Variables
2. appsettings.json
3. User Secrets (development)
4. Configuration providers (DI)

## Deployment Targets

### Supported Platforms

- **Windows**: Windows Server 2019+, Windows 10+
- **Linux**: Ubuntu 20.04+, Debian 11+, RHEL 8+
- **macOS**: macOS 11.0+
- **Docker**: Container-based deployments
- **Kubernetes**: Cloud-native orchestration
- **Azure**: App Service, Functions, Container Apps
- **AWS**: ECS, Lambda, Elastic Beanstalk

### Distribution

- **NuGet.org**: Public package distribution
- **Private NuGet Feed**: Enterprise customers
- **GitHub Releases**: Source and binaries
- **Docker Hub**: Container images (optional)

