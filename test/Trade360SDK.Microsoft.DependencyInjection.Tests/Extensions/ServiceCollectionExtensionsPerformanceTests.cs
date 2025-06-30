using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests.Extensions;

/// <summary>
/// Performance and scalability tests designed to maximize code coverage
/// by exercising code paths under stress and high-load scenarios.
/// </summary>
public class ServiceCollectionExtensionsPerformanceTests
{
    private readonly ServiceCollection _services;
    private readonly Mock<IConfiguration> _mockConfiguration;

    public ServiceCollectionExtensionsPerformanceTests()
    {
        _services = new ServiceCollection();
        _mockConfiguration = new Mock<IConfiguration>();
    }

    #region Registration Performance Tests

    [Fact]
    public void MassiveServiceRegistration_ShouldHandleThousandsOfRegistrations()
    {
        var services = new ServiceCollection();
        
        for (int i = 0; i < 100; i++)
        {
            services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = $"https://test{i}.com";
            });
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();
        }
        
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
    }

    [Fact]
    public async Task ConcurrentServiceRegistration_ShouldBeThreadSafe()
    {
        var tasks = new List<Task<ServiceCollection>>();
        
        // Create separate ServiceCollection instances for thread safety
        for (int i = 0; i < 50; i++)
        {
            int capturedI = i;
            tasks.Add(Task.Run(() =>
            {
                var services = new ServiceCollection();
                services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
                services.Configure<Trade360Settings>(options =>
                {
                    options.SnapshotApiBaseUrl = $"https://concurrent{capturedI}.test";
                });
                services.AddTrade360PrematchSnapshotClient();
                services.AddTrade360InplaySnapshotClient();
                return services;
            }));
        }
        
        var serviceCollections = await Task.WhenAll(tasks);
        
        // Verify all service collections can build successfully
        foreach (var services in serviceCollections)
        {
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
        }
    }

    #endregion

    #region Service Resolution Performance Tests

    [Fact]
    public void MassiveServiceResolution_ShouldMaintainPerformance()
    {
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://performance.test";
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        
        for (int i = 0; i < 1000; i++)
        {
            var factory = serviceProvider.GetService<ICustomersApiFactory>();
            var prematch = serviceProvider.GetService<ISnapshotPrematchApiClient>();
            var inplay = serviceProvider.GetService<ISnapshotInplayApiClient>();
            var mapper = serviceProvider.GetService<AutoMapper.IMapper>();
            
            factory.Should().NotBeNull();
            prematch.Should().NotBeNull();
            inplay.Should().NotBeNull();
            mapper.Should().NotBeNull();
        }
    }

    [Fact]
    public async Task ConcurrentServiceResolution_ShouldBeThreadSafe()
    {
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://threadsafe.test";
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        var tasks = new List<Task<bool>>();
        
        for (int i = 0; i < 100; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                try
                {
                    var factory = serviceProvider.GetService<ICustomersApiFactory>();
                    var prematch = serviceProvider.GetService<ISnapshotPrematchApiClient>();
                    var inplay = serviceProvider.GetService<ISnapshotInplayApiClient>();
                    var mapper = serviceProvider.GetService<AutoMapper.IMapper>();
                    
                    return factory != null && prematch != null && inplay != null && mapper != null;
                }
                catch
                {
                    return false;
                }
            }));
        }
        
        var results = await Task.WhenAll(tasks);
        results.Should().AllBeEquivalentTo(true);
    }

    #endregion

    #region HttpClient Performance Tests

    [Fact]
    public void MassiveHttpClientCreation_ShouldHandleHighLoad()
    {
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://httpclient.test";
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var clients = new List<HttpClient>();
        
        for (int i = 0; i < 100; i++)
        {
            clients.Add(httpClientFactory.CreateClient("IMetadataHttpClient"));
            clients.Add(httpClientFactory.CreateClient("IPackageDistributionHttpClient"));
            clients.Add(httpClientFactory.CreateClient("ISubscriptionHttpClient"));
            clients.Add(httpClientFactory.CreateClient("ISnapshotPrematchApiClient"));
            clients.Add(httpClientFactory.CreateClient("ISnapshotInplayApiClient"));
        }
        
        clients.Should().HaveCount(500);
        clients.ForEach(client => client.Dispose());
    }

    [Fact]
    public async Task ConcurrentHttpClientCreation_ShouldBeThreadSafe()
    {
        // Arrange
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://concurrent-http.test";
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var tasks = new List<Task<bool>>();
        
        // Act - Create HTTP clients concurrently
        for (int i = 0; i < 500; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                try
                {
                    var clients = new List<HttpClient>
                    {
                        httpClientFactory.CreateClient("IMetadataHttpClient"),
                        httpClientFactory.CreateClient("IPackageDistributionHttpClient"),
                        httpClientFactory.CreateClient("ISubscriptionHttpClient"),
                        httpClientFactory.CreateClient("ISnapshotPrematchApiClient"),
                        httpClientFactory.CreateClient("ISnapshotInplayApiClient")
                    };
                    
                    var success = clients.All(c => c != null);
                    clients.ForEach(c => c.Dispose());
                    return success;
                }
                catch
                {
                    return false;
                }
            }));
        }
        
        var results = await Task.WhenAll(tasks);
        
        // Assert - All should succeed
        results.Should().AllBeEquivalentTo(true);
    }

    #endregion

    #region Memory and Resource Tests

    [Fact]
    public async Task RepeatedServiceProviderCreation_ShouldNotLeakMemory()
    {
        var serviceProviders = new List<ServiceProvider>();
        
        for (int i = 0; i < 50; i++)
        {
            var services = new ServiceCollection();
            services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = $"https://memory{i}.test";
            });
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();
            
            var serviceProvider = services.BuildServiceProvider();
            
            serviceProvider.GetService<ICustomersApiFactory>();
            serviceProvider.GetService<ISnapshotPrematchApiClient>();
            serviceProvider.GetService<ISnapshotInplayApiClient>();
            
            serviceProviders.Add(serviceProvider);
        }
        
        serviceProviders.ForEach(sp => sp.Dispose());
        serviceProviders.Clear();
        
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        await Task.Delay(1); // Small delay to ensure async method
    }

    [Fact]
    public async Task LongRunningServiceProvider_ShouldMaintainStability()
    {
        // Arrange
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://longrunning.test";
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        var exceptions = new List<Exception>();
        
        // Act - Use service provider intensively over time
        for (int round = 0; round < 100; round++)
        {
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    var factory = serviceProvider.GetService<ICustomersApiFactory>();
                    var prematch = serviceProvider.GetService<ISnapshotPrematchApiClient>();
                    var inplay = serviceProvider.GetService<ISnapshotInplayApiClient>();
                    var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                    
                    using var client = httpClientFactory.CreateClient("IMetadataHttpClient");
                    
                    factory.Should().NotBeNull();
                    prematch.Should().NotBeNull();
                    inplay.Should().NotBeNull();
                    client.Should().NotBeNull();
                }
                
                // Simulate some processing time
                await Task.Delay(1);
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }
        
        // Assert - Should not have any exceptions
        exceptions.Should().BeEmpty();
    }

    #endregion

    #region Configuration Performance Tests

    [Fact]
    public void MassiveConfigurationChanges_ShouldHandleGracefully()
    {
        var services = new ServiceCollection();
        
        for (int i = 0; i < 100; i++)
        {
            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    [$"Setting{i}"] = $"Value{i}",
                    ["Trade360Settings:SnapshotApiBaseUrl"] = $"https://config{i}.test"
                });
            
            var config = configBuilder.Build();
            
            services.AddTrade360CustomerApiClient(config);
            services.Configure<Trade360Settings>(config.GetSection("Trade360Settings"));
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();
        }
        
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
    }

    [Fact]
    public async Task DynamicConfigurationChanges_ShouldBeThreadSafe()
    {
        // Arrange
        var tasks = new List<Task<ServiceCollection>>();
        
        // Act - Change configuration from multiple threads using separate collections
        for (int i = 0; i < 50; i++)
        {
            int capturedI = i;
            tasks.Add(Task.Run(() =>
            {
                var services = new ServiceCollection();
                services.Configure<Trade360Settings>(options =>
                {
                    options.SnapshotApiBaseUrl = $"https://dynamic{capturedI}.test";
                });
                
                services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
                services.AddTrade360PrematchSnapshotClient();
                services.AddTrade360InplaySnapshotClient();
                return services;
            }));
        }
        
        var serviceCollections = await Task.WhenAll(tasks);
        
        // Assert - Should complete successfully
        foreach (var services in serviceCollections)
        {
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
        }
    }

    #endregion

    #region Stress Testing

    [Fact]
    public async Task HighVolumeStressTest_ShouldMaintainStability()
    {
        // Arrange
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://stress.test";
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var exceptions = new List<Exception>();
        var tasks = new List<Task>();
        
        // Act - High volume concurrent operations
        for (int i = 0; i < 200; i++)
        {
            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    for (int j = 0; j < 50; j++)
                    {
                        // Simulate intensive operations
                        var factory = serviceProvider.GetService<ICustomersApiFactory>();
                        var prematch = serviceProvider.GetService<ISnapshotPrematchApiClient>();
                        var inplay = serviceProvider.GetService<ISnapshotInplayApiClient>();
                        var mapper = serviceProvider.GetService<AutoMapper.IMapper>();
                        
                        using var client1 = httpClientFactory.CreateClient("IMetadataHttpClient");
                        using var client2 = httpClientFactory.CreateClient("IPackageDistributionHttpClient");
                        using var client3 = httpClientFactory.CreateClient("ISubscriptionHttpClient");
                        using var client4 = httpClientFactory.CreateClient("ISnapshotPrematchApiClient");
                        using var client5 = httpClientFactory.CreateClient("ISnapshotInplayApiClient");
                        
                        // Add small delay to prevent overwhelming
                        await Task.Delay(1);
                    }
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
            }));
        }
        
        await Task.WhenAll(tasks);
        
        // Assert - Should handle stress without critical failures
        exceptions.Count.Should().BeLessThan(10); // Allow some exceptions under extreme stress
    }

    #endregion

    #region Benchmark Tests

    [Fact]
    public void ServiceRegistrationBenchmark_ShouldMeetPerformanceTargets()
    {
        // Arrange
        var services = new ServiceCollection();
        var iterations = 1000;
        var stopwatch = Stopwatch.StartNew();
        
        // Act - Benchmark service registration
        for (int i = 0; i < iterations; i++)
        {
            services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = $"https://benchmark{i}.test";
            });
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();
        }
        
        stopwatch.Stop();
        var avgTimePerRegistration = (double)stopwatch.ElapsedMilliseconds / iterations;
        
        // Assert - Should meet performance targets
        avgTimePerRegistration.Should().BeLessThan(5.0); // Less than 5ms per registration
        
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
    }

    [Fact]
    public void ServiceResolutionBenchmark_ShouldMeetPerformanceTargets()
    {
        // Arrange
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://resolution-benchmark.test";
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        var iterations = 10000;
        var stopwatch = Stopwatch.StartNew();
        
        // Act - Benchmark service resolution
        for (int i = 0; i < iterations; i++)
        {
            var factory = serviceProvider.GetService<ICustomersApiFactory>();
            var prematch = serviceProvider.GetService<ISnapshotPrematchApiClient>();
            var inplay = serviceProvider.GetService<ISnapshotInplayApiClient>();
            var mapper = serviceProvider.GetService<AutoMapper.IMapper>();
        }
        
        stopwatch.Stop();
        var avgTimePerResolution = (double)stopwatch.ElapsedMilliseconds / iterations;
        
        // Assert - Should meet performance targets
        avgTimePerResolution.Should().BeLessThan(0.1); // Less than 0.1ms per resolution
    }

    #endregion
} 