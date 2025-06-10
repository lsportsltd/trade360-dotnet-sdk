using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests;

public class ServiceCollectionExtensionsActualTests
{
    [Fact]
    public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldExecuteActualValidation()
    {
        var services = new ServiceCollection();

        var act = () => services.AddTrade360CustomerApiClient(null!);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("configuration");
    }


}
