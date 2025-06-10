using FluentAssertions;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.SnapshotApi;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace Trade360SDK.SnapshotApi.Tests;

public class SnapshotHttpClientActualTests
{
    [Fact]
    public void SnapshotInplayApiClient_Constructor_WithValidParameters_ShouldCreateInstance()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var mockMapper = new Mock<IMapper>();
        var mockOptions = new Mock<IOptions<Trade360Settings>>();
        var settings = new Trade360Settings 
        { 
            SnapshotApiBaseUrl = "https://api.test.com",
            InplayPackageCredentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 }
        };
        mockOptions.Setup(x => x.Value).Returns(settings);

        var act = () => new SnapshotInplayApiClient(mockFactory.Object, mockOptions.Object, mockMapper.Object);

        act.Should().NotThrow();
    }

    [Fact]
    public void SnapshotPrematchApiClient_Constructor_WithValidParameters_ShouldCreateInstance()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var mockMapper = new Mock<IMapper>();
        var mockOptions = new Mock<IOptions<Trade360Settings>>();
        var settings = new Trade360Settings 
        { 
            SnapshotApiBaseUrl = "https://api.test.com",
            PrematchPackageCredentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 }
        };
        mockOptions.Setup(x => x.Value).Returns(settings);

        var act = () => new SnapshotPrematchApiClient(mockFactory.Object, mockOptions.Object, mockMapper.Object);

        act.Should().NotThrow();
    }
}
