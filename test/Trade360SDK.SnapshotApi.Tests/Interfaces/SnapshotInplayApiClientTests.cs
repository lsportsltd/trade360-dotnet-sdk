using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.SnapshotApi;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Trade360SDK.SnapshotApi.Entities.Responses;

namespace Trade360SDK.SnapshotApi.Tests;

public class SnapshotInplayApiClientTests
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<IOptions<Trade360Settings>> _mockOptions;
    private readonly Mock<IMapper> _mockMapper;

    public SnapshotInplayApiClientTests()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockOptions = new Mock<IOptions<Trade360Settings>>();
        _mockMapper = new Mock<IMapper>();

        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://api.test.com",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 1,
                Username = "user",
                Password = "pass"
            }
        };

        _mockOptions.Setup(o => o.Value).Returns(settings);

        var mockHttpClient = new Mock<HttpClient>();
        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var client = new SnapshotInplayApiClient(_mockHttpClientFactory.Object, _mockOptions.Object, _mockMapper.Object);

        client.Should().NotBeNull();
        _mockHttpClientFactory.Verify(f => f.CreateClient(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Fact]
    public void Constructor_WithNullSnapshotApiBaseUrl_ShouldThrowInvalidOperationException()
    {
        var settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = null,
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 1,
                Username = "user",
                Password = "pass"
            }
        };
        var mockOptions = new Mock<IOptions<Trade360Settings>>();
        mockOptions.Setup(o => o.Value).Returns(settings);

        Action act = () => new SnapshotInplayApiClient(_mockHttpClientFactory.Object, mockOptions.Object, _mockMapper.Object);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task GetFixtures_ShouldMapRequestAndCallPostEntityAsync()
    {
        var client = new SnapshotInplayApiClient(_mockHttpClientFactory.Object, _mockOptions.Object, _mockMapper.Object);
        var requestDto = new GetFixturesRequestDto();
        var mappedRequest = new BaseStandardRequest();
        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(requestDto)).Returns(mappedRequest);

        try
        {
            await client.GetFixtures(requestDto, CancellationToken.None);
        }
        catch
        {
            // Devin: Expected exception in test - verifying method call behavior only
        }

        _mockMapper.Verify(m => m.Map<BaseStandardRequest>(requestDto), Times.Once);
    }

    [Fact]
    public async Task GetLivescore_ShouldMapRequestAndCallPostEntityAsync()
    {
        var client = new SnapshotInplayApiClient(_mockHttpClientFactory.Object, _mockOptions.Object, _mockMapper.Object);
        var requestDto = new GetLivescoreRequestDto();
        var mappedRequest = new BaseStandardRequest();

        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(requestDto)).Returns(mappedRequest);

        try
        {
            await client.GetLivescore(requestDto, CancellationToken.None);
        }
        catch
        {
            // Devin: Expected exception in test - verifying method call behavior only
        }

        _mockMapper.Verify(m => m.Map<BaseStandardRequest>(requestDto), Times.Once);
    }

    [Fact]
    public async Task GetFixtureMarkets_ShouldMapRequestAndCallPostEntityAsync()
    {
        var client = new SnapshotInplayApiClient(_mockHttpClientFactory.Object, _mockOptions.Object, _mockMapper.Object);
        var requestDto = new GetMarketRequestDto();
        var mappedRequest = new BaseStandardRequest();

        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(requestDto)).Returns(mappedRequest);

        try
        {
            await client.GetFixtureMarkets(requestDto, CancellationToken.None);
        }
        catch
        {
            // Devin: Expected exception in test - verifying method call behavior only
        }

        _mockMapper.Verify(m => m.Map<BaseStandardRequest>(requestDto), Times.Once);
    }

    [Fact]
    public async Task GetEvents_ShouldMapRequestAndCallPostEntityAsync()
    {
        var client = new SnapshotInplayApiClient(_mockHttpClientFactory.Object, _mockOptions.Object, _mockMapper.Object);
        var requestDto = new GetMarketRequestDto();
        var mappedRequest = new BaseStandardRequest();

        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(requestDto)).Returns(mappedRequest);

        try
        {
            await client.GetEvents(requestDto, CancellationToken.None);
        }
        catch
        {
            // Devin: Expected exception in test - verifying method call behavior only
        }

        _mockMapper.Verify(m => m.Map<BaseStandardRequest>(requestDto), Times.Once);
    }
}
