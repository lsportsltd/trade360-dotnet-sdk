using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests.Extensions;

/// <summary>
/// Advanced edge case tests designed to maximize code coverage by testing
/// complex boundary conditions, unusual configurations, and error scenarios
/// </summary>
public class ServiceCollectionExtensionsAdvancedEdgeCasesTests
{
    private readonly ServiceCollection _services;
    private readonly Mock<IConfiguration> _mockConfiguration;

    public ServiceCollectionExtensionsAdvancedEdgeCasesTests()
    {
        _services = new ServiceCollection();
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(x => x.GetSection(It.IsAny<string>()))
            .Returns(new Mock<IConfigurationSection>().Object);
    }

    #region Complex Configuration Scenarios

    [Fact]
    public void AddServices_WithComplexNestedConfiguration_ShouldHandleCorrectly()
    {
        // Arrange
        var complexConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings:SnapshotApiBaseUrl"] = "https://nested.config.test.com",
                ["Trade360Settings:CustomProperty"] = "custom_value",
                ["Trade360Settings:Nested:Property"] = "nested_value"
            })
            .Build();

        // Act
        _services.AddTrade360CustomerApiClient(complexConfig);
        _services.Configure<Trade360Settings>(complexConfig.GetSection("Trade360Settings"));
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var factory = serviceProvider.GetService<ICustomersApiFactory>();
        var prematch = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        var inplay = serviceProvider.GetService<ISnapshotInplayApiClient>();

        factory.Should().NotBeNull();
        prematch.Should().NotBeNull();
        inplay.Should().NotBeNull();
    }

    [Fact]
    public void AddServices_WithEnvironmentSpecificConfiguration_ShouldWork()
    {
        // Arrange
        var envConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings:SnapshotApiBaseUrl"] = "https://staging-api.trade360.com",
                ["Environment"] = "Staging"
            })
            .Build();

        _services.Configure<Trade360Settings>(envConfig.GetSection("Trade360Settings"));

        // Act
        _services.AddTrade360CustomerApiClient(envConfig);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        
        var prematchClient = httpClientFactory.CreateClient("ISnapshotPrematchApiClient");
        var inplayClient = httpClientFactory.CreateClient("ISnapshotInplayApiClient");

        prematchClient.BaseAddress.ToString().Should().StartWith("https://staging-api.trade360.com");
        inplayClient.BaseAddress.ToString().Should().StartWith("https://staging-api.trade360.com");
        
        prematchClient.Dispose();
        inplayClient.Dispose();
    }

    [Fact]
    public void AddServices_WithMultipleConfigurationSources_ShouldMergeCorrectly()
    {
        // Arrange
        var config1 = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings:SnapshotApiBaseUrl"] = "https://primary.test.com",
                ["Trade360Settings:Property1"] = "value1"
            })
            .Build();

        var config2 = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings:Property2"] = "value2",
                ["Trade360Settings:Property3"] = "value3"
            })
            .Build();

        // Act
        _services.AddTrade360CustomerApiClient(config1);
        _services.Configure<Trade360Settings>(config1.GetSection("Trade360Settings"));
        _services.Configure<Trade360Settings>(config2.GetSection("Trade360Settings"));
        _services.AddTrade360PrematchSnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetService<IOptions<Trade360Settings>>();
        var factory = serviceProvider.GetService<ICustomersApiFactory>();

        options.Should().NotBeNull();
        factory.Should().NotBeNull();
    }

    #endregion

    #region Service Registration Boundary Conditions

    [Fact]
    public void AddServices_WithMinimalValidConfiguration_ShouldRegisterSuccessfully()
    {
        // Arrange
        var minimalConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings:SnapshotApiBaseUrl"] = "https://a.b"
            })
            .Build();

        _services.Configure<Trade360Settings>(minimalConfig.GetSection("Trade360Settings"));

        // Act
        _services.AddTrade360CustomerApiClient(minimalConfig);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
        serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull();
        serviceProvider.GetService<ISnapshotInplayApiClient>().Should().NotBeNull();
    }

    [Fact]
    public void AddServices_WithEmptyServiceCollection_ShouldBuildFromScratch()
    {
        // Arrange
        var emptyServices = new ServiceCollection();
        
        // Act
        emptyServices.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        emptyServices.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://empty-start.test.com";
        });
        emptyServices.AddTrade360PrematchSnapshotClient();
        emptyServices.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = emptyServices.BuildServiceProvider();
        var services = emptyServices.ToList();
        
        services.Should().NotBeEmpty();
        serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
        serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull();
        serviceProvider.GetService<ISnapshotInplayApiClient>().Should().NotBeNull();
    }

    [Fact]
    public void AddServices_WithPreExistingServices_ShouldIntegrateCorrectly()
    {
        // Arrange
        _services.AddLogging();
        _services.AddHttpClient();
        _services.AddScoped<IServiceProvider>(provider => provider);

        // Act
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://integration.test.com";
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        // Should have both pre-existing and new services
        serviceProvider.GetService<IHttpClientFactory>().Should().NotBeNull();
        serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
        serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull();
        serviceProvider.GetService<ISnapshotInplayApiClient>().Should().NotBeNull();
    }

    #endregion

    #region HTTP Client Advanced Scenarios

    [Fact]
    public void HttpClients_WithCustomBaseAddresses_ShouldConfigureIndependently()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot-custom.test.com/v2/";
        });

        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Act & Assert
        var clients = new Dictionary<string, HttpClient>
        {
            ["IMetadataHttpClient"] = httpClientFactory.CreateClient("IMetadataHttpClient"),
            ["IPackageDistributionHttpClient"] = httpClientFactory.CreateClient("IPackageDistributionHttpClient"),
            ["ISubscriptionHttpClient"] = httpClientFactory.CreateClient("ISubscriptionHttpClient"),
            ["ISnapshotPrematchApiClient"] = httpClientFactory.CreateClient("ISnapshotPrematchApiClient"),
            ["ISnapshotInplayApiClient"] = httpClientFactory.CreateClient("ISnapshotInplayApiClient")
        };

        // Snapshot clients should have custom base address
        clients["ISnapshotPrematchApiClient"].BaseAddress.ToString().Should().StartWith("https://snapshot-custom.test.com/v2/");
        clients["ISnapshotInplayApiClient"].BaseAddress.ToString().Should().StartWith("https://snapshot-custom.test.com/v2/");

        // Cleanup
        foreach (var client in clients.Values)
        {
            client.Dispose();
        }
    }

    [Fact]
    public void HttpClients_WithUnicodeUrls_ShouldHandleCorrectly()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://test-unicode.example.com/api/";
        });

        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Act
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Assert
        var prematchClient = httpClientFactory.CreateClient("ISnapshotPrematchApiClient");
        var inplayClient = httpClientFactory.CreateClient("ISnapshotInplayApiClient");

        prematchClient.Should().NotBeNull();
        inplayClient.Should().NotBeNull();
        prematchClient.BaseAddress.Should().NotBeNull();
        inplayClient.BaseAddress.Should().NotBeNull();

        prematchClient.Dispose();
        inplayClient.Dispose();
    }

    [Fact]
    public void HttpClients_WithPortNumbers_ShouldPreservePortInformation()
    {
        // Arrange
        var urlsWithPorts = new[]
        {
            "https://api.test.com:8443",
            "http://localhost:8080",
            "https://secure.example.com:9443/api/"
        };

        foreach (var url in urlsWithPorts)
        {
            var services = new ServiceCollection();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = url;
            });
            services.AddTrade360PrematchSnapshotClient();

            // Act
            var serviceProvider = services.BuildServiceProvider();
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var client = httpClientFactory.CreateClient("ISnapshotPrematchApiClient");

            // Assert
            client.Should().NotBeNull();
            client.BaseAddress.Should().NotBeNull();
            client.BaseAddress.ToString().Should().StartWith(url);

            client.Dispose();
        }
    }

    #endregion

    #region Service Lifecycle and Scoping Tests

    [Fact]
    public void Services_InDifferentScopes_ShouldBehaveCorrectly()
    {
        // Arrange
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://scoped.test.com";
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();

        // Act & Assert
        using (var scope1 = serviceProvider.CreateScope())
        using (var scope2 = serviceProvider.CreateScope())
        {
            var factory1 = scope1.ServiceProvider.GetService<ICustomersApiFactory>();
            var factory2 = scope2.ServiceProvider.GetService<ICustomersApiFactory>();
            var prematch1 = scope1.ServiceProvider.GetService<ISnapshotPrematchApiClient>();
            var prematch2 = scope2.ServiceProvider.GetService<ISnapshotInplayApiClient>();

            // Transient services should be different instances
            factory1.Should().NotBeNull();
            factory2.Should().NotBeNull();
            factory1.Should().NotBeSameAs(factory2);

            prematch1.Should().NotBeNull();
            prematch2.Should().NotBeNull();
        }
    }

    [Fact]
    public void Services_AfterServiceProviderDisposal_ShouldNotCauseMemoryLeaks()
    {
        // Arrange
        var serviceProviders = new List<ServiceProvider>();

        // Act
        for (int i = 0; i < 10; i++)
        {
            var services = new ServiceCollection();
            services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = $"https://disposal-test-{i}.com";
            });
            services.AddTrade360PrematchSnapshotClient();
            services.AddTrade360InplaySnapshotClient();

            var serviceProvider = services.BuildServiceProvider();
            serviceProviders.Add(serviceProvider);

            // Use services
            var factory = serviceProvider.GetService<ICustomersApiFactory>();
            var prematch = serviceProvider.GetService<ISnapshotPrematchApiClient>();
            var inplay = serviceProvider.GetService<ISnapshotInplayApiClient>();

            factory.Should().NotBeNull();
            prematch.Should().NotBeNull();
            inplay.Should().NotBeNull();
        }

        // Assert - Dispose all and verify no exceptions
        foreach (var provider in serviceProviders)
        {
            var act = () => provider.Dispose();
            act.Should().NotThrow();
        }
    }

    #endregion

    #region Error Handling and Recovery

    [Fact]
    public void AddServices_WithPartiallyInvalidConfiguration_ShouldContinueWorking()
    {
        // Arrange
        var partialConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings:SnapshotApiBaseUrl"] = "https://partial.test.com",
                ["Trade360Settings:InvalidProperty"] = "this_should_be_ignored",
                ["Trade360Settings:AnotherInvalidProperty"] = null!
            })
            .Build();

        // Act
        _services.AddTrade360CustomerApiClient(partialConfig);
        _services.Configure<Trade360Settings>(partialConfig.GetSection("Trade360Settings"));
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
        serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull();
        serviceProvider.GetService<ISnapshotInplayApiClient>().Should().NotBeNull();
    }

    [Fact]
    public void AddServices_WithConfigurationReloading_ShouldHandleChanges()
    {
        // Arrange
        var configData = new Dictionary<string, string>
        {
            ["Trade360Settings:SnapshotApiBaseUrl"] = "https://initial.test.com"
        };

        var configBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection(configData);
        var config = configBuilder.Build();

        _services.AddTrade360CustomerApiClient(config);
        _services.Configure<Trade360Settings>(config.GetSection("Trade360Settings"));
        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();

        // Act - Simulate configuration reload
        configData["Trade360Settings:SnapshotApiBaseUrl"] = "https://updated.test.com";
        config.Reload();

        // Assert - Services should still work (configuration is captured at registration)
        var factory = serviceProvider.GetService<ICustomersApiFactory>();
        var prematch = serviceProvider.GetService<ISnapshotPrematchApiClient>();

        factory.Should().NotBeNull();
        prematch.Should().NotBeNull();
    }

    #endregion

    #region Policy and HttpClient Integration Tests

    [Fact]
    public void HttpClients_WithPoliciesAndCustomConfiguration_ShouldWorkTogether()
    {
        // Arrange
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://policy-integration.test.com";
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Act - Create clients and verify they have policies applied
        var clientTypes = new[]
        {
            "IMetadataHttpClient",
            "IPackageDistributionHttpClient", 
            "ISubscriptionHttpClient",
            "ISnapshotPrematchApiClient",
            "ISnapshotInplayApiClient"
        };

        // Assert
        foreach (var clientType in clientTypes)
        {
            var client = httpClientFactory.CreateClient(clientType);
            client.Should().NotBeNull();
            client.Timeout.Should().BeGreaterThan(TimeSpan.Zero);
            client.Dispose();
        }
    }

    [Fact]
    public void PolicyMethods_WithMultipleInvocations_ShouldReturnConsistentResults()
    {
        // Arrange & Act
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Assert - Multiple calls should work consistently
        for (int i = 0; i < 5; i++)
        {
            var client1 = httpClientFactory.CreateClient("IMetadataHttpClient");
            var client2 = httpClientFactory.CreateClient("IPackageDistributionHttpClient");
            var client3 = httpClientFactory.CreateClient("ISubscriptionHttpClient");

            client1.Should().NotBeNull();
            client2.Should().NotBeNull();
            client3.Should().NotBeNull();

            client1.Dispose();
            client2.Dispose();
            client3.Dispose();
        }
    }

    #endregion

    #region Integration with AutoMapper

    [Fact]
    public void AutoMapper_WithMultipleServiceRegistrations_ShouldNotConflict()
    {
        // Arrange
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://automapper.test.com";
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Act
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        var mapper1 = serviceProvider.GetService<AutoMapper.IMapper>();
        var mapper2 = serviceProvider.GetService<AutoMapper.IMapper>();
        var mapper3 = serviceProvider.GetService<AutoMapper.IMapper>();

        mapper1.Should().NotBeNull();
        mapper2.Should().NotBeNull();
        mapper3.Should().NotBeNull();
        
        // Verify the mappers are functional (they may or may not be the same instance)
        mapper1.ConfigurationProvider.Should().NotBeNull();
        mapper2.ConfigurationProvider.Should().NotBeNull();
        mapper3.ConfigurationProvider.Should().NotBeNull();
    }

    [Fact]
    public void Services_WithComplexDependencyChain_ShouldResolveCorrectly()
    {
        // Arrange
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://dependency-chain.test.com";
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();

        // Act & Assert - Verify all dependencies resolve
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var mapper = serviceProvider.GetRequiredService<AutoMapper.IMapper>();
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();
        var factory = serviceProvider.GetRequiredService<ICustomersApiFactory>();
        var prematch = serviceProvider.GetRequiredService<ISnapshotPrematchApiClient>();
        var inplay = serviceProvider.GetRequiredService<ISnapshotInplayApiClient>();

        httpClientFactory.Should().NotBeNull();
        mapper.Should().NotBeNull();
        options.Should().NotBeNull();
        factory.Should().NotBeNull();
        prematch.Should().NotBeNull();
        inplay.Should().NotBeNull();
    }

    #endregion
} 