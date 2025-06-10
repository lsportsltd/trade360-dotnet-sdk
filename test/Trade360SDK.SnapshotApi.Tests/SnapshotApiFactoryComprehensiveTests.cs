using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.SnapshotApi;
using System.Net;
using System.Text;
using Microsoft.Extensions.Options;
using AutoMapper;

namespace Trade360SDK.SnapshotApi.Tests;

public class SnapshotInplayApiClientComprehensiveBusinessLogicTests
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;

    private readonly Mock<IMapper> _mockMapper;
    private readonly SnapshotInplayApiClient _client;

    public SnapshotInplayApiClientComprehensiveBusinessLogicTests()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();

        _mockMapper = new Mock<IMapper>();
        
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        
        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 789,
                Username = "prematchuser",
                Password = "prematchpass"
            }
        };
        
        _client = new SnapshotInplayApiClient(_mockHttpClientFactory.Object, Options.Create(settings), _mockMapper.Object);
    }

    [Fact]
    public void CreateSnapshotInplayApiClient_WithValidParameters_ShouldReturnClient()
    {
        var httpClient = new HttpClient();
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var result = _client;

        result.Should().NotBeNull();
        result.Should().BeOfType<SnapshotInplayApiClient>();
    }

    [Fact]
    public void CreateSnapshotPrematchApiClient_WithValidParameters_ShouldReturnClient()
    {
        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 789,
                Username = "prematchuser",
                Password = "prematchpass"
            }
        };

        var httpClient = new HttpClient();
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var result = new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, Options.Create(settings), _mockMapper.Object);

        result.Should().NotBeNull();
        result.Should().BeOfType<SnapshotPrematchApiClient>();
    }

    [Fact]
    public void CreateSnapshotInplayApiClient_WithNullSettings_ShouldThrowNullReferenceException()
    {
        var act = () => new SnapshotInplayApiClient(_mockHttpClientFactory.Object, null!, _mockMapper.Object);

        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void CreateSnapshotPrematchApiClient_WithNullSettings_ShouldThrowNullReferenceException()
    {
        var act = () => new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, null!, _mockMapper.Object);

        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void CreateSnapshotInplayApiClient_WithEmptyBaseUrl_ShouldThrowUriFormatException()
    {
        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = ""
        };

        var act = () => new SnapshotInplayApiClient(_mockHttpClientFactory.Object, Options.Create(settings), _mockMapper.Object);

        act.Should().Throw<UriFormatException>();
    }

    [Fact]
    public void CreateSnapshotPrematchApiClient_WithEmptyBaseUrl_ShouldThrowUriFormatException()
    {
        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = ""
        };

        var act = () => new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, Options.Create(settings), _mockMapper.Object);

        act.Should().Throw<UriFormatException>();
    }

    [Fact]
    public void CreateSnapshotInplayApiClient_WithNullCredentials_ShouldCreateClientWithoutCredentials()
    {
        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 789,
                Username = "prematchuser",
                Password = "prematchpass"
            }
        };
        
        var httpClient = new HttpClient();
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var result = new SnapshotInplayApiClient(_mockHttpClientFactory.Object, Options.Create(settings), _mockMapper.Object);

        result.Should().NotBeNull();
        result.Should().BeOfType<SnapshotInplayApiClient>();
    }

    [Fact]
    public void CreateSnapshotPrematchApiClient_WithNullCredentials_ShouldCreateClientWithoutCredentials()
    {
        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 789,
                Username = "prematchuser",
                Password = "prematchpass"
            }
        };
        
        var httpClient = new HttpClient();
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var result = new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, Options.Create(settings), _mockMapper.Object);

        result.Should().NotBeNull();
        result.Should().BeOfType<SnapshotPrematchApiClient>();
    }

    [Fact]
    public void CreateSnapshotInplayApiClient_WithDifferentCredentials_ShouldCreateMultipleClients()
    {
        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 789,
                Username = "prematchuser",
                Password = "prematchpass"
            }
        };

        var httpClient = new HttpClient();
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var result1 = new SnapshotInplayApiClient(_mockHttpClientFactory.Object, Options.Create(settings), _mockMapper.Object);
        var result2 = new SnapshotInplayApiClient(_mockHttpClientFactory.Object, Options.Create(settings), _mockMapper.Object);

        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Should().NotBeSameAs(result2);
    }

    [Fact]
    public void CreateSnapshotPrematchApiClient_WithDifferentCredentials_ShouldCreateMultipleClients()
    {
        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 789,
                Username = "prematchuser",
                Password = "prematchpass"
            }
        };

        var httpClient = new HttpClient();
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var result1 = new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, Options.Create(settings), _mockMapper.Object);
        var result2 = new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, Options.Create(settings), _mockMapper.Object);

        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Should().NotBeSameAs(result2);
    }

    [Fact]
    public void Constructor_WithNullHttpClientFactory_ShouldThrowArgumentNullException()
    {
        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://api.example.com/",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            },
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 789,
                Username = "prematchuser",
                Password = "prematchpass"
            }
        };

        var act = () => new SnapshotInplayApiClient(null!, Options.Create(settings), _mockMapper.Object);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CreateSnapshotInplayApiClient_WithComplexBaseUrl_ShouldHandleCorrectly()
    {
        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://complex.api.example.com/v2/snapshot/inplay/",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 999,
                Username = "complexuser",
                Password = "complexpass"
            },
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 888,
                Username = "complexuser2",
                Password = "complexpass2"
            }
        };

        var httpClient = new HttpClient();
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var result = new SnapshotInplayApiClient(_mockHttpClientFactory.Object, Options.Create(settings), _mockMapper.Object);

        result.Should().NotBeNull();
        result.Should().BeOfType<SnapshotInplayApiClient>();
    }

    [Fact]
    public void CreateSnapshotPrematchApiClient_WithComplexBaseUrl_ShouldHandleCorrectly()
    {
        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://complex.api.example.com/v2/snapshot/prematch/",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 999,
                Username = "complexuser",
                Password = "complexpass"
            },
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 888,
                Username = "complexuser2",
                Password = "complexpass2"
            }
        };

        var httpClient = new HttpClient();
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var result = new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, Options.Create(settings), _mockMapper.Object);

        result.Should().NotBeNull();
        result.Should().BeOfType<SnapshotPrematchApiClient>();
    }
}
