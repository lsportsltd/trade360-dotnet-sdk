using FluentAssertions;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ;
using Xunit;

namespace Trade360SDK.Feed.Tests.Configuration;

public class RabbitMqFeedConsumeQueueNameTests
{
    [Fact]
    public void ResolveConsumeQueueName_Default_ReturnsUnderscorePackagePattern()
    {
        var settings = new RmqConnectionSettings { PackageId = 3265 };

        var name = RabbitMqFeed.ResolveConsumeQueueName(settings);

        name.Should().Be("_3265_");
    }

    [Fact]
    public void ResolveConsumeQueueName_WithZeroPackageId_UsesCustomQueueName()
    {
        var settings = new RmqConnectionSettings
        {
            PackageId = 0,
            CustomQueueName = "fixed-queue"
        };

        RabbitMqFeed.ResolveConsumeQueueName(settings).Should().Be("fixed-queue");
    }

    [Fact]
    public void ResolveConsumeQueueName_CustomQueueName_ReplacesDefault()
    {
        var settings = new RmqConnectionSettings
        {
            PackageId = 1,
            CustomQueueName = "my-enterprise-queue"
        };

        RabbitMqFeed.ResolveConsumeQueueName(settings).Should().Be("my-enterprise-queue");
    }

    [Fact]
    public void ResolveConsumeQueueName_CustomQueueName_TrimsWhitespace()
    {
        var settings = new RmqConnectionSettings
        {
            PackageId = 1,
            CustomQueueName = "  trim-me  "
        };

        RabbitMqFeed.ResolveConsumeQueueName(settings).Should().Be("trim-me");
    }
}
