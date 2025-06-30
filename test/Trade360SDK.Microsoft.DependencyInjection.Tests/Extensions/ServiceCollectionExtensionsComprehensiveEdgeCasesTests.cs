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
/// Extremely comprehensive edge case tests designed to achieve maximum code coverage
/// by testing every possible scenario, error path, and boundary condition.
/// </summary>
public class ServiceCollectionExtensionsComprehensiveEdgeCasesTests
{
    private readonly ServiceCollection _services;
    private readonly Mock<IConfiguration> _mockConfiguration;

    public ServiceCollectionExtensionsComprehensiveEdgeCasesTests()
    {
        _services = new ServiceCollection();
        _mockConfiguration = new Mock<IConfiguration>();
    }

    #region Configuration Edge Cases

    [Fact]
    public void AddTrade360CustomerApiClient_WithEmptyConfiguration_ShouldNotThrow()
    {
        // Arrange
        var emptyConfiguration = new ConfigurationBuilder().Build();
        
        // Act & Assert
        var act = () => _services.AddTrade360CustomerApiClient(emptyConfiguration);
        act.Should().NotThrow();
        
        var result = _services.AddTrade360CustomerApiClient(emptyConfiguration);
        result.Should().BeSameAs(_services);
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithNullSectionConfiguration_ShouldNotThrow()
    {
        // Arrange
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(x => x.GetSection(It.IsAny<string>())).Returns((IConfigurationSection)null);
        
        // Act & Assert
        var act = () => _services.AddTrade360CustomerApiClient(mockConfiguration.Object);
        act.Should().NotThrow();
    }

    [Fact]
    public void MultipleRegistrations_SameConfiguration_ShouldHandleGracefully()
    {
        // Act - Register the same configuration multiple times
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        
        // Assert - Should not throw and build successfully
        var serviceProvider = _services.BuildServiceProvider();
        var factory = serviceProvider.GetService<ICustomersApiFactory>();
        factory.Should().NotBeNull();
    }

    [Fact]
    public void SnapshotClients_WithNullSettings_ShouldRegisterButRequireConfiguration()
    {
        // Arrange - Don't configure any settings, which means SnapshotApiBaseUrl will be null
        
        // Act - Services should register without throwing during registration
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();
        
        // Assert - Services register successfully but require configuration to function
        var serviceProvider = _services.BuildServiceProvider();
        
        // The services are registered but will throw when accessed without proper configuration
        var act1 = () => serviceProvider.GetService<ISnapshotPrematchApiClient>();
        var act2 = () => serviceProvider.GetService<ISnapshotInplayApiClient>();
        
        // These will throw because SnapshotApiBaseUrl is not configured
        act1.Should().Throw<InvalidOperationException>().WithMessage("*SnapshotApiBaseUrl*");
        act2.Should().Throw<InvalidOperationException>().WithMessage("*SnapshotApiBaseUrl*");
    }

    #endregion

    #region HTTP Client Edge Cases

    [Theory]
    [InlineData("ftp://invalid-protocol.com")]
    [InlineData("ldap://invalid-protocol.com")]
    [InlineData("file:///local/path")]
    public void HttpClient_WithNonHttpProtocols_ShouldCreateClient(string invalidUrl)
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = invalidUrl;
        });
        _services.AddTrade360PrematchSnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        
        // Act - HttpClient creation succeeds even with non-HTTP protocols
        var client = httpClientFactory.CreateClient("ISnapshotPrematchApiClient");
        
        // Assert - Client is created but has the invalid base address (Uri may add trailing slash)
        client.Should().NotBeNull();
        client.BaseAddress.ToString().Should().StartWith(invalidUrl);
        
        client.Dispose();
    }

    [Theory]
    [InlineData("://missing-scheme")]
    [InlineData("http://")]
    [InlineData("https://")]
    public void HttpClient_WithMalformedUrls_ShouldThrowException(string malformedUrl)
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = malformedUrl;
        });
        _services.AddTrade360InplaySnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        
        // Act & Assert
        var act = () => httpClientFactory.CreateClient("ISnapshotInplayApiClient");
        act.Should().Throw<UriFormatException>();
    }

    [Fact]
    public void HttpClientFactory_CreateMultipleInstances_ShouldWorkCorrectly()
    {
        // Arrange
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://test.com";
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        
        // Act - Create multiple instances of each client type
        var clients = new List<HttpClient>();
        for (int i = 0; i < 10; i++)
        {
            clients.Add(httpClientFactory.CreateClient("IMetadataHttpClient"));
            clients.Add(httpClientFactory.CreateClient("IPackageDistributionHttpClient"));
            clients.Add(httpClientFactory.CreateClient("ISubscriptionHttpClient"));
            clients.Add(httpClientFactory.CreateClient("ISnapshotPrematchApiClient"));
            clients.Add(httpClientFactory.CreateClient("ISnapshotInplayApiClient"));
        }
        
        // Assert
        clients.Should().HaveCount(50);
        clients.Should().AllSatisfy(client => client.Should().NotBeNull());
        
        // Cleanup
        clients.ForEach(client => client.Dispose());
    }

    #endregion

    #region Service Registration Edge Cases

    [Fact]
    public void ServiceCollection_WithExistingHttpClientFactory_ShouldNotConflict()
    {
        // Arrange - Pre-register HttpClientFactory
        _services.AddHttpClient();
        
        // Act
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        
        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        httpClientFactory.Should().NotBeNull();
    }

    [Fact]
    public void ServiceCollection_WithExistingAutoMapper_ShouldNotConflict()
    {
        // Arrange - Pre-register AutoMapper
        _services.AddAutoMapper(typeof(ServiceCollectionExtensionsComprehensiveEdgeCasesTests));
        
        // Act
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options => options.SnapshotApiBaseUrl = "https://test.com");
        _services.AddTrade360PrematchSnapshotClient();
        
        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var mapper = serviceProvider.GetService<AutoMapper.IMapper>();
        mapper.Should().NotBeNull();
    }

    [Fact]
    public void ServiceCollection_WithCustomServiceProvider_ShouldWork()
    {
        // Arrange
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options => options.SnapshotApiBaseUrl = "https://custom.test");
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();
        
        // Act - Build with different scopes
        using var scope1 = _services.BuildServiceProvider().CreateScope();
        using var scope2 = _services.BuildServiceProvider().CreateScope();
        
        // Assert
        var factory1 = scope1.ServiceProvider.GetService<ICustomersApiFactory>();
        var factory2 = scope2.ServiceProvider.GetService<ICustomersApiFactory>();
        
        factory1.Should().NotBeNull();
        factory2.Should().NotBeNull();
        factory1.Should().NotBeSameAs(factory2); // Scoped instances
    }

    #endregion

    #region Options Pattern Edge Cases

    [Fact]
    public void Trade360Settings_WithDefaultValues_ShouldWork()
    {
        // Arrange - Configure with default/empty settings
        _services.Configure<Trade360Settings>(options => { });
        _services.AddTrade360PrematchSnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetService<IOptions<Trade360Settings>>();
        
        // Assert
        options.Should().NotBeNull();
        options.Value.Should().NotBeNull();
    }

    [Fact]
    public void Trade360Settings_WithComplexConfiguration_ShouldWork()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://complex-config.test.com:8443/api/v2/";
        });
        
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        
        // Act
        var prematchClient = httpClientFactory.CreateClient("ISnapshotPrematchApiClient");
        var inplayClient = httpClientFactory.CreateClient("ISnapshotInplayApiClient");
        
        // Assert
        prematchClient.BaseAddress.Should().Be("https://complex-config.test.com:8443/api/v2/");
        inplayClient.BaseAddress.Should().Be("https://complex-config.test.com:8443/api/v2/");
    }

    [Fact]
    public void OptionsMonitor_WithDynamicChanges_ShouldWork()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360Settings:SnapshotApiBaseUrl"] = "https://initial.test.com"
            })
            .Build();
        
        _services.Configure<Trade360Settings>(configuration.GetSection("Trade360Settings"));
        _services.AddTrade360PrematchSnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        var optionsMonitor = serviceProvider.GetService<IOptionsMonitor<Trade360Settings>>();
        
        // Act & Assert
        optionsMonitor.Should().NotBeNull();
        optionsMonitor.CurrentValue.SnapshotApiBaseUrl.Should().Be("https://initial.test.com");
    }

    #endregion

    #region Concurrency and Threading Edge Cases

    [Fact]
    public async Task ConcurrentServiceCreation_ShouldNotCauseRaceConditions()
    {
        // Arrange
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options => options.SnapshotApiBaseUrl = "https://concurrent.test");
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        
        // Act - Create services concurrently
        var tasks = new List<Task<bool>>();
        for (int i = 0; i < 50; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                try
                {
                    var client1 = httpClientFactory.CreateClient("IMetadataHttpClient");
                    var client2 = httpClientFactory.CreateClient("ISnapshotPrematchApiClient");
                    var client3 = httpClientFactory.CreateClient("ISnapshotInplayApiClient");
                    
                    var factory = serviceProvider.GetService<ICustomersApiFactory>();
                    var prematch = serviceProvider.GetService<ISnapshotPrematchApiClient>();
                    var inplay = serviceProvider.GetService<ISnapshotInplayApiClient>();
                    
                    return client1 != null && client2 != null && client3 != null &&
                           factory != null && prematch != null && inplay != null;
                }
                catch
                {
                    return false;
                }
            }));
        }
        
        var results = await Task.WhenAll(tasks);
        
        // Assert
        results.Should().AllBeEquivalentTo(true);
    }

    [Fact]
    public async Task HttpClient_WithCancellationToken_ShouldHandleCorrectly()
    {
        // Arrange
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var client = httpClientFactory.CreateClient("IMetadataHttpClient");
        
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1));
        
        // Act & Assert
        var request = new HttpRequestMessage(HttpMethod.Get, "https://httpbin.org/delay/5");
        
        var act = async () => await client.SendAsync(request, cts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    #endregion

    #region Boundary Value Testing

    [Theory]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("abc")]
    public void HttpClient_WithMinimalValidUrls_ShouldWork(string subdomain)
    {
        // Arrange
        var url = $"https://{subdomain}.co";
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = url;
        });
        _services.AddTrade360PrematchSnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        
        // Act & Assert
        var client = httpClientFactory.CreateClient("ISnapshotPrematchApiClient");
        client.BaseAddress.ToString().Should().StartWith(url);
    }

    [Fact]
    public void HttpClient_WithMaximumUrlLength_ShouldWork()
    {
        // Arrange - Create very long but valid URL
        var longSubdomain = new string('a', 63); // Max subdomain length
        var url = $"https://{longSubdomain}.example.com/very/long/path/with/many/segments";
        
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = url;
        });
        _services.AddTrade360InplaySnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        
        // Act & Assert
        var client = httpClientFactory.CreateClient("ISnapshotInplayApiClient");
        client.BaseAddress.ToString().Should().StartWith(url);
    }

    #endregion

    #region Error Recovery and Resilience

    [Fact]
    public void ServiceProvider_AfterDisposal_ShouldHandleGracefully()
    {
        // Arrange
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        var serviceProvider = _services.BuildServiceProvider();
        
        // Act - Dispose and try to use
        serviceProvider.Dispose();
        
        // Assert - Should not crash, but may throw ObjectDisposedException
        var act = () => serviceProvider.GetService<ICustomersApiFactory>();
        act.Should().Throw<ObjectDisposedException>();
    }

    [Fact]
    public void HttpClientFactory_WithServiceProviderScope_ShouldWork()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options => options.SnapshotApiBaseUrl = "https://scope.test");
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();
        
        var serviceProvider = _services.BuildServiceProvider();
        
        // Act - Test with multiple scopes
        using (var scope1 = serviceProvider.CreateScope())
        using (var scope2 = serviceProvider.CreateScope())
        {
            var factory1 = scope1.ServiceProvider.GetService<IHttpClientFactory>();
            var factory2 = scope2.ServiceProvider.GetService<IHttpClientFactory>();
            
            var client1 = factory1.CreateClient("ISnapshotPrematchApiClient");
            var client2 = factory2.CreateClient("ISnapshotInplayApiClient");
            
            // Assert
            client1.Should().NotBeNull();
            client2.Should().NotBeNull();
            client1.Should().NotBeSameAs(client2);
        }
    }

    #endregion

    #region Integration and System Testing

    [Fact]
    public void CompleteIntegration_AllServices_ShouldWorkTogether()
    {
        // Arrange - Register everything
        _services.AddTrade360CustomerApiClient(_mockConfiguration.Object);
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://integration.test.com";
        });
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();
        
        // Add additional services for comprehensive testing
        _services.AddLogging();
        _services.AddOptions();
        
        var serviceProvider = _services.BuildServiceProvider();
        
        // Act - Verify all services can be resolved and used
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var customersFactory = serviceProvider.GetService<ICustomersApiFactory>();
        var prematchService = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        var inplayService = serviceProvider.GetService<ISnapshotInplayApiClient>();
        var mapper = serviceProvider.GetService<AutoMapper.IMapper>();
        var options = serviceProvider.GetService<IOptions<Trade360Settings>>();
        
        // Assert - Everything should work together
        httpClientFactory.Should().NotBeNull();
        customersFactory.Should().NotBeNull();
        prematchService.Should().NotBeNull();
        inplayService.Should().NotBeNull();
        mapper.Should().NotBeNull();
        options.Should().NotBeNull();
        
        // Test HTTP clients can be created
        var metadataClient = httpClientFactory.CreateClient("IMetadataHttpClient");
        var distributionClient = httpClientFactory.CreateClient("IPackageDistributionHttpClient");
        var subscriptionClient = httpClientFactory.CreateClient("ISubscriptionHttpClient");
        var prematchClient = httpClientFactory.CreateClient("ISnapshotPrematchApiClient");
        var inplayClient = httpClientFactory.CreateClient("ISnapshotInplayApiClient");
        
        metadataClient.Should().NotBeNull();
        distributionClient.Should().NotBeNull();
        subscriptionClient.Should().NotBeNull();
        prematchClient.Should().NotBeNull();
        inplayClient.Should().NotBeNull();
        
        // Verify base addresses are set correctly for snapshot clients
        prematchClient.BaseAddress.Should().Be("https://integration.test.com/");
        inplayClient.BaseAddress.Should().Be("https://integration.test.com/");
    }

    #endregion
} 