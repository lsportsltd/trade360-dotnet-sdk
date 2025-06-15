using FluentAssertions;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Http;
using AutoMapper;

namespace Trade360SDK.CustomersApi.Tests;

public class MetadataHttpClientActualTests
{
    [Fact]
    public void MetadataHttpClient_Constructor_WithValidParameters_ShouldCreateInstance()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, "https://api.test.com", credentials, mockMapper.Object);

        act.Should().NotThrow();
    }

    [Fact]
    public void MetadataHttpClient_Constructor_WithHttpsUrl_ShouldCreateInstance()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, "https://secure-api.test.com/v1", credentials, mockMapper.Object);

        act.Should().NotThrow();
    }

    [Fact]
    public void MetadataHttpClient_Constructor_WithComplexCredentials_ShouldCreateInstance()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var credentials = new PackageCredentials 
        { 
            Username = "complex-user@domain.com", 
            Password = "P@ssw0rd!123", 
            PackageId = 999 
        };
        var mockMapper = new Mock<IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, "https://api.test.com", credentials, mockMapper.Object);

        act.Should().NotThrow();
    }
}
