using FluentAssertions;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Feed.Configuration;

namespace Trade360SDK.Feed.Tests;

public class IFeedFactoryTests
{
    [Fact]
    public void CreateFeed_ShouldReturnIFeed()
    {
        var mockFactory = new Mock<IFeedFactory>();
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "pass"
        };
        var trade360Settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.test.com",
            SnapshotApiBaseUrl = "https://snapshot.test.com"
        };
        var mockFeed = new Mock<IFeed>();

        mockFactory.Setup(f => f.CreateFeed(settings, trade360Settings, FlowType.InPlay))
                  .Returns(mockFeed.Object);

        var result = mockFactory.Object.CreateFeed(settings, trade360Settings, FlowType.InPlay);

        result.Should().NotBeNull();
        result.Should().Be(mockFeed.Object);
        mockFactory.Verify(f => f.CreateFeed(settings, trade360Settings, FlowType.InPlay), Times.Once);
    }

    [Theory]
    [InlineData(FlowType.InPlay)]
    [InlineData(FlowType.PreMatch)]
    public void CreateFeed_WithDifferentFlowTypes_ShouldHandleCorrectly(FlowType flowType)
    {
        var mockFactory = new Mock<IFeedFactory>();
        var settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            PackageId = 1,
            UserName = "user",
            Password = "pass"
        };
        var trade360Settings = new Trade360Settings();
        var mockFeed = new Mock<IFeed>();

        mockFactory.Setup(f => f.CreateFeed(settings, trade360Settings, flowType))
                  .Returns(mockFeed.Object);

        var result = mockFactory.Object.CreateFeed(settings, trade360Settings, flowType);

        result.Should().NotBeNull();
        mockFactory.Verify(f => f.CreateFeed(settings, trade360Settings, flowType), Times.Once);
    }
}
