# Project Brief

## Official .NET SDK for LSports Trade360 Services

The Trade360 .NET SDK is a comprehensive client library that simplifies integration with LSports Trade360 ecosystem. It provides developers with a robust, type-safe interface to consume real-time sports data feeds, manage subscriptions, and access metadata through RESTful APIs.

## Core Objectives

1. **Real-time Data Consumption**: Enable customers to receive live sports data updates via RabbitMQ message broker
2. **High Throughput**: Handle high-volume data streams with automatic recovery and connection management
3. **Type Safety**: Provide strongly-typed entities for all message types and API responses
4. **Developer Experience**: Simple, intuitive API surface with dependency injection support
5. **Reliability**: Automatic reconnection, snapshot recovery, and health monitoring

## Primary Responsibilities

- Real-time sports data consumption via RabbitMQ message broker
- HTTP-based snapshot recovery for data consistency
- Customer subscription and metadata management
- Automatic connection recovery and health monitoring
- Message routing and deserialization with strong typing

## Role in Trade360 Ecosystem

This SDK serves as the primary integration point for Trade360 customers, enabling them to receive live sports data updates (fixtures, livescore, markets, settlements) and manage their data subscriptions programmatically.

## Success Metrics

- **Performance**: Handle 10,000+ messages per second
- **Reliability**: 99.9% uptime with automatic recovery
- **Developer Adoption**: Clear documentation, comprehensive samples
- **Type Coverage**: 100% of message types strongly typed
- **Test Coverage**: 80%+ code coverage with unit and integration tests

## Target Audience

- .NET developers integrating sports betting platforms
- Backend systems consuming real-time sports data
- Applications requiring live odds and market data
- Services needing subscription and metadata management

