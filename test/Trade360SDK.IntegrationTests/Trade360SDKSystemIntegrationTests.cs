using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;
using Trade360SDK.SnapshotApi.Interfaces;
using Xunit;

namespace Trade360SDK.IntegrationTests
{
    /// <summary>
    /// Comprehensive system integration tests for the entire Trade360 SDK covering
    /// end-to-end scenarios, performance validation, and system-level behaviors.
    /// </summary>
    public class Trade360SDKSystemIntegrationTests : IDisposable
    {
        private readonly ServiceCollection _services;
        private readonly Mock<ILoggerFactory> _mockLoggerFactory;
        private readonly Mock<ILogger> _mockLogger;
        private readonly Trade360Settings _validSettings;

        public Trade360SDKSystemIntegrationTests()
        {
            _services = new ServiceCollection();
            _mockLoggerFactory = new Mock<ILoggerFactory>();
            _mockLogger = new Mock<ILogger>();

            _mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<Type>())).Returns(_mockLogger.Object);
            _mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_mockLogger.Object);

            _validSettings = new Trade360Settings
            {
                CustomersApiBaseUrl = "https://test-api.example.com",
                SnapshotApiBaseUrl = "https://test-snapshot.example.com",
                InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 12345,
                    Username = "test_inplay_user",
                    Password = "test_inplay_password",
                    MessageFormat = "json"
                },
                PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 67890,
                    Username = "test_prematch_user",
                    Password = "test_prematch_password",
                    MessageFormat = "json"
                }
            };
        }

        [Fact]
        public void SDK_ServiceRegistration_ShouldRegisterAllRequiredServices()
        {
            // Arrange
            _services.AddLogging();
            _services.AddSingleton(_mockLoggerFactory.Object);

            // Act
            _services.AddTrade360CustomersApiClient(_validSettings.CustomersApiBaseUrl!, _validSettings.InplayPackageCredentials!);
            _services.AddTrade360PrematchSnapshotClient();
            _services.AddTrade360InplaySnapshotClient();

            var serviceProvider = _services.BuildServiceProvider();

            // Assert
            serviceProvider.GetService<IMetadataHttpClient>().Should().NotBeNull();
            serviceProvider.GetService<ISubscriptionHttpClient>().Should().NotBeNull();
            serviceProvider.GetService<IPackageDistributionHttpClient>().Should().NotBeNull();
            serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull();
            serviceProvider.GetService<ISnapshotInplayApiClient>().Should().NotBeNull();
        }

        [Fact]
        public void SDK_ServiceRegistration_WithMultipleRegistrations_ShouldHandleCorrectly()
        {
            // Arrange
            _services.AddLogging();
            _services.AddSingleton(_mockLoggerFactory.Object);

            // Act - Register services multiple times to test idempotency
            for (int i = 0; i < 3; i++)
            {
                _services.AddTrade360CustomersApiClient(_validSettings.CustomersApiBaseUrl!, _validSettings.InplayPackageCredentials!);
                _services.AddTrade360PrematchSnapshotClient();
                _services.AddTrade360InplaySnapshotClient();
            }

            var serviceProvider = _services.BuildServiceProvider();

            // Assert - Should not throw and services should be available
            var metadataClients = serviceProvider.GetServices<IMetadataHttpClient>();
            metadataClients.Should().NotBeEmpty();

            var prematchClients = serviceProvider.GetServices<ISnapshotPrematchApiClient>();
            prematchClients.Should().NotBeEmpty();

            var inplayClients = serviceProvider.GetServices<ISnapshotInplayApiClient>();
            inplayClients.Should().NotBeEmpty();
        }

        [Fact]
        public void SDK_ServiceResolution_PerformanceTest()
        {
            // Arrange
            _services.AddLogging();
            _services.AddSingleton(_mockLoggerFactory.Object);
            _services.AddTrade360CustomersApiClient(_validSettings.CustomersApiBaseUrl!, _validSettings.InplayPackageCredentials!);
            _services.AddTrade360PrematchSnapshotClient();
            _services.AddTrade360InplaySnapshotClient();

            var serviceProvider = _services.BuildServiceProvider();
            const int iterations = 1000;

            // Act
            var stopwatch = Stopwatch.StartNew();
            
            for (int i = 0; i < iterations; i++)
            {
                var metadataClient = serviceProvider.GetService<IMetadataHttpClient>();
                var subscriptionClient = serviceProvider.GetService<ISubscriptionHttpClient>();
                var prematchClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();
                var inplayClient = serviceProvider.GetService<ISnapshotInplayApiClient>();

                // Verify services are resolved
                metadataClient.Should().NotBeNull();
                subscriptionClient.Should().NotBeNull();
                prematchClient.Should().NotBeNull();
                inplayClient.Should().NotBeNull();
            }

            stopwatch.Stop();

            // Assert - Should resolve services quickly (less than 1 second for 1000 iterations)
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000, 
                "Service resolution should be fast enough for high-throughput scenarios");
        }

        [Fact]
        public async Task SDK_ConcurrentServiceUsage_ShouldBeThreadSafe()
        {
            // Arrange
            _services.AddLogging();
            _services.AddSingleton(_mockLoggerFactory.Object);
            _services.AddTrade360CustomersApiClient(_validSettings.CustomersApiBaseUrl!, _validSettings.InplayPackageCredentials!);
            _services.AddTrade360PrematchSnapshotClient();
            _services.AddTrade360InplaySnapshotClient();

            var serviceProvider = _services.BuildServiceProvider();
            const int concurrentTasks = 50;
            var tasks = new List<Task>();

            // Act - Simulate concurrent service access
            for (int i = 0; i < concurrentTasks; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    // Resolve services concurrently
                    var metadataClient = serviceProvider.GetService<IMetadataHttpClient>();
                    var subscriptionClient = serviceProvider.GetService<ISubscriptionHttpClient>();
                    var prematchClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();
                    var inplayClient = serviceProvider.GetService<ISnapshotInplayApiClient>();

                    // Verify all services are resolved
                    metadataClient.Should().NotBeNull();
                    subscriptionClient.Should().NotBeNull();
                    prematchClient.Should().NotBeNull();
                    inplayClient.Should().NotBeNull();
                }));
            }

            // Assert - All concurrent operations should complete successfully
            var act = async () => await Task.WhenAll(tasks);
            await act.Should().NotThrowAsync();
        }

        public void Dispose()
        {
            // Clean up any resources
            _services.Clear();
        }
    }
}
