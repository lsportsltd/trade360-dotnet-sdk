using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.SnapshotApi.Extensions;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.SnapshotApi.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterServices()
    {
        var services = new ServiceCollection();

        var result = services.AddTrade360PrematchSnapshotClient();

        result.Should().BeSameAs(services);

        var registeredServices = services.Where(s => s.ServiceType == typeof(ISnapshotPrematchApiClient)).ToList();
        registeredServices.Should().HaveCountGreaterOrEqualTo(1);
        var concreteRegistration = registeredServices.FirstOrDefault(s => s.ImplementationType != null);
        concreteRegistration.Should().NotBeNull();
        concreteRegistration!.ImplementationType.Should().Be(typeof(SnapshotPrematchApiClient));
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterServices()
    {
        var services = new ServiceCollection();

        var result = services.AddTrade360InplaySnapshotClient();

        result.Should().BeSameAs(services);

        var registeredServices = services.Where(s => s.ServiceType == typeof(ISnapshotInplayApiClient)).ToList();
        registeredServices.Should().HaveCountGreaterOrEqualTo(1);
        var concreteRegistration = registeredServices.FirstOrDefault(s => s.ImplementationType != null);
        concreteRegistration.Should().NotBeNull();
        concreteRegistration!.ImplementationType.Should().Be(typeof(SnapshotInplayApiClient));
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

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterAutoMapper()
    {
        var services = new ServiceCollection();

        services.AddTrade360PrematchSnapshotClient();

        var autoMapperRegistrations = services.Where(s => s.ServiceType.Name.Contains("Mapper")).ToList();
        autoMapperRegistrations.Should().NotBeEmpty();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterAutoMapper()
    {
        var services = new ServiceCollection();

        services.AddTrade360InplaySnapshotClient();

        var autoMapperRegistrations = services.Where(s => s.ServiceType.Name.Contains("Mapper")).ToList();
        autoMapperRegistrations.Should().NotBeEmpty();
    }
}
