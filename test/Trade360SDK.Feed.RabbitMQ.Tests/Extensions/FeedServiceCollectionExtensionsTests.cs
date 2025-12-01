using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.Feed;
using Trade360SDK.Feed.FeedType;
using Trade360SDK.Feed.RabbitMQ.Extensions;
using Trade360SDK.Feed.RabbitMQ.Resolvers;

namespace Trade360SDK.Feed.RabbitMQ.Tests;

public class FeedServiceCollectionExtensionsTests
{
    [Fact]
    public void AddT360RmqFeedSdk_ShouldRegisterAllServices()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        var result = services.AddT360RmqFeedSdk(configuration);

        result.Should().BeSameAs(services);
        
        var serviceProvider = services.BuildServiceProvider();
        
        serviceProvider.GetService<IFeedFactory>().Should().NotBeNull();
        serviceProvider.GetService<IFeedFactory>().Should().BeOfType<RabbitMqFeedFactory>();
    }

    [Fact]
    public void AddT360RmqFeedSdk_ShouldRegisterMessageProcessors()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddT360RmqFeedSdk(configuration);

        var registeredServices = services.Where(s => s.ServiceType == typeof(IMessageProcessor)).ToList();
        
        registeredServices.Should().HaveCountGreaterThan(10);
    }

    [Fact]
    public void AddT360RmqFeedSdk_ShouldRegisterMessageProcessorContainers()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddT360RmqFeedSdk(configuration);

        var serviceProvider = services.BuildServiceProvider();
        
        serviceProvider.GetService<MessageProcessorContainer<InPlay>>().Should().NotBeNull();
        serviceProvider.GetService<MessageProcessorContainer<PreMatch>>().Should().NotBeNull();
    }
}
