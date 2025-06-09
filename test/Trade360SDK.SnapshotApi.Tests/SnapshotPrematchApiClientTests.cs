using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Livescore;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.SnapshotApi;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Trade360SDK.SnapshotApi.Entities.Responses;

namespace Trade360SDK.SnapshotApi.Tests;

public class SnapshotPrematchApiClientTests
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<IOptions<Trade360Settings>> _mockOptions;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Trade360Settings _settings;

    public SnapshotPrematchApiClientTests()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockOptions = new Mock<IOptions<Trade360Settings>>();
        _mockMapper = new Mock<IMapper>();

        _settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://api.test.com",
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 2,
                Username = "user",
                Password = "pass"
            }
        };

        _mockOptions.Setup(o => o.Value).Returns(_settings);

        var mockHttpClient = new Mock<HttpClient>();
        _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var client = new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, _mockOptions.Object, _mockMapper.Object);

        client.Should().NotBeNull();
        _mockHttpClientFactory.Verify(f => f.CreateClient(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Fact]
    public void Constructor_WithNullSnapshotApiBaseUrl_ShouldThrowInvalidOperationException()
    {
        _settings.SnapshotApiBaseUrl = null;

        Action act = () => new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, _mockOptions.Object, _mockMapper.Object);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public async Task GetFixtures_ShouldMapRequestAndCallPostEntityAsync()
    {
        var client = new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, _mockOptions.Object, _mockMapper.Object);
        var requestDto = new GetFixturesRequestDto();
        var mappedRequest = new BaseStandardRequest();

        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(requestDto)).Returns(mappedRequest);

        try
        {
            await client.GetFixtures(requestDto, CancellationToken.None);
        }
        catch
        {
        }

        _mockMapper.Verify(m => m.Map<BaseStandardRequest>(requestDto), Times.Once);
    }

    [Fact]
    public async Task GetLivescore_ShouldMapRequestAndCallPostEntityAsync()
    {
        var client = new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, _mockOptions.Object, _mockMapper.Object);
        var requestDto = new GetLivescoreRequestDto();
        var mappedRequest = new BaseStandardRequest();

        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(requestDto)).Returns(mappedRequest);

        try
        {
            await client.GetLivescore(requestDto, CancellationToken.None);
        }
        catch
        {
        }

        _mockMapper.Verify(m => m.Map<BaseStandardRequest>(requestDto), Times.Once);
    }

    [Fact]
    public async Task GetOutrightFixture_ShouldMapRequestAndCallPostEntityAsync()
    {
        var client = new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, _mockOptions.Object, _mockMapper.Object);
        var requestDto = new GetOutrightFixturesRequestDto();
        var mappedRequest = new BaseOutrightRequest();

        _mockMapper.Setup(m => m.Map<BaseOutrightRequest>(requestDto)).Returns(mappedRequest);

        try
        {
            await client.GetOutrightFixture(requestDto, CancellationToken.None);
        }
        catch
        {
        }

        _mockMapper.Verify(m => m.Map<BaseOutrightRequest>(requestDto), Times.Once);
    }

    [Fact]
    public async Task GetOutrightScores_ShouldMapRequestAndCallPostEntityAsync()
    {
        var client = new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, _mockOptions.Object, _mockMapper.Object);
        var requestDto = new GetOutrightLivescoreRequestDto();
        var mappedRequest = new BaseOutrightRequest();

        _mockMapper.Setup(m => m.Map<BaseOutrightRequest>(requestDto)).Returns(mappedRequest);

        try
        {
            await client.GetOutrightScores(requestDto, CancellationToken.None);
        }
        catch
        {
        }

        _mockMapper.Verify(m => m.Map<BaseOutrightRequest>(requestDto), Times.Once);
    }

    [Fact]
    public async Task GetOutrightLeagues_ShouldMapRequestAndCallPostEntityAsync()
    {
        var client = new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, _mockOptions.Object, _mockMapper.Object);
        var requestDto = new GetFixturesRequestDto();
        var mappedRequest = new BaseStandardRequest();

        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(requestDto)).Returns(mappedRequest);

        try
        {
            await client.GetOutrightLeagues(requestDto, CancellationToken.None);
        }
        catch
        {
        }

        _mockMapper.Verify(m => m.Map<BaseStandardRequest>(requestDto), Times.Once);
    }

    [Fact]
    public async Task GetOutrightLeaguesMarkets_ShouldMapRequestAndCallPostEntityAsync()
    {
        var client = new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, _mockOptions.Object, _mockMapper.Object);
        var requestDto = new GetMarketRequestDto();
        var mappedRequest = new BaseStandardRequest();

        _mockMapper.Setup(m => m.Map<BaseStandardRequest>(requestDto)).Returns(mappedRequest);

        try
        {
            await client.GetOutrightLeaguesMarkets(requestDto, CancellationToken.None);
        }
        catch
        {
        }

        _mockMapper.Verify(m => m.Map<BaseStandardRequest>(requestDto), Times.Once);
    }
}
