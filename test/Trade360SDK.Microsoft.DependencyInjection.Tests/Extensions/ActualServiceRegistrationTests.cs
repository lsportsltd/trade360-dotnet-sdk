using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests;

public class ActualServiceRegistrationTests
{
    [Fact]
    public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldExecuteActualValidation()
    {
        var services = new ServiceCollection();

        var act = () => services.AddTrade360CustomerApiClient(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithValidConfiguration_ShouldExecuteActualRegistration()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        var result = services.AddTrade360CustomerApiClient(configuration);

        result.Should().NotBeNull();
        result.Should().BeSameAs(services);
        services.Should().NotBeEmpty();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldExecuteActualRegistration()
    {
        var services = new ServiceCollection();

        var result = services.AddTrade360PrematchSnapshotClient();

        result.Should().NotBeNull();
        result.Should().BeSameAs(services);
        services.Should().NotBeEmpty();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldExecuteActualRegistration()
    {
        var services = new ServiceCollection();

        var result = services.AddTrade360InplaySnapshotClient();

        result.Should().NotBeNull();
        result.Should().BeSameAs(services);
        services.Should().NotBeEmpty();
    }
}
