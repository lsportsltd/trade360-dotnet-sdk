using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;
using Trade360SDK.SnapshotApi;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddTrade360CustomerApiClient_ShouldRegisterServices()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        var result = services.AddTrade360CustomerApiClient(configuration);

        result.Should().BeSameAs(services);

        var registeredServices = services.Where(s => s.ServiceType == typeof(ICustomersApiFactory)).ToList();
        registeredServices.Should().HaveCountGreaterOrEqualTo(1);
        registeredServices.First().ImplementationType.Should().Be(typeof(CustomersApiFactory));
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldRegisterHttpClients()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddTrade360CustomerApiClient(configuration);

        var httpClientRegistrations = services.Where(s => s.ServiceType.Name.Contains("HttpClient")).ToList();
        httpClientRegistrations.Should().NotBeEmpty();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldRegisterAutoMapper()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddTrade360CustomerApiClient(configuration);

        var autoMapperRegistrations = services.Where(s => s.ServiceType.Name.Contains("Mapper")).ToList();
        autoMapperRegistrations.Should().NotBeEmpty();
    }



    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterServices()
    {
        var services = new ServiceCollection();

        var result = services.AddTrade360PrematchSnapshotClient();

        result.Should().BeSameAs(services);
        
        // Verify the service is registered without instantiation
        var registeredServices = services.Where(s => s.ServiceType == typeof(ISnapshotPrematchApiClient)).ToList();
        registeredServices.Should().NotBeEmpty();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterServices()
    {
        var services = new ServiceCollection();

        var result = services.AddTrade360InplaySnapshotClient();

        result.Should().BeSameAs(services);
        
        // Verify the service is registered without instantiation
        var registeredServices = services.Where(s => s.ServiceType == typeof(ISnapshotInplayApiClient)).ToList();
        registeredServices.Should().NotBeEmpty();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterHttpClient()
    {
        var services = new ServiceCollection();

        services.AddTrade360PrematchSnapshotClient();

        var httpClientRegistrations = services.Where(s => s.ServiceType.Name.Contains("HttpClient")).ToList();
        httpClientRegistrations.Should().NotBeEmpty();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterHttpClient()
    {
        var services = new ServiceCollection();

        services.AddTrade360InplaySnapshotClient();

        var httpClientRegistrations = services.Where(s => s.ServiceType.Name.Contains("HttpClient")).ToList();
        httpClientRegistrations.Should().NotBeEmpty();
    }
}
