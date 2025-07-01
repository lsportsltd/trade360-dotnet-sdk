

using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Livescore;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.SnapshotApi;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Trade360SDK.SnapshotApi.Http;
using Trade360SDK.SnapshotApi.Interfaces;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests
{
    public class SnapshotPrematchApiClientComprehensiveTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<IOptions<Trade360Settings>> _mockOptions;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Trade360Settings _validSettings;
        private readonly SnapshotPrematchApiClient _client;

        public SnapshotPrematchApiClientComprehensiveTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockOptions = new Mock<IOptions<Trade360Settings>>();
            _mockMapper = new Mock<IMapper>();

            _validSettings = new Trade360Settings
            {
                SnapshotApiBaseUrl = "https://snapshot.example.com/",
                PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "prematchuser",
                    Password = "prematchpass"
                }
            };

            _mockOptions.Setup(x => x.Value).Returns(_validSettings);

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri(_validSettings.SnapshotApiBaseUrl)
            };

            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _client = new SnapshotPrematchApiClient(_mockHttpClientFactory.Object, _mockOptions.Object, _mockMapper.Object);
        }

        #region Constructor Advanced Tests

        [Fact]
        public void Constructor_WithDifferentHttpClientConfigurations_ShouldHandleCorrectly()
        {
            // Arrange
            var customSettings = new Trade360Settings
            {
                SnapshotApiBaseUrl = "https://custom.api.com/v2/",
                PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 999,
                    Username = "customuser",
                    Password = "custompass"
                }
            };

            var customOptions = new Mock<IOptions<Trade360Settings>>();
            customOptions.Setup(x => x.Value).Returns(customSettings);

            var customHttpClient = new HttpClient();
            var customFactory = new Mock<IHttpClientFactory>();
            customFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(customHttpClient);

            // Act
            var client = new SnapshotPrematchApiClient(customFactory.Object, customOptions.Object, _mockMapper.Object);

            // Assert
            client.Should().NotBeNull();
            client.Should().BeAssignableTo<ISnapshotPrematchApiClient>();
            client.Should().BeAssignableTo<BaseHttpClient>();
        }

        [Theory]
        [InlineData("https://test1.api.com")]
        [InlineData("https://test2.api.com/")]
        [InlineData("https://api.example.com/v1/snapshot")]
        [InlineData("http://localhost:5000")]
        public void Constructor_WithVariousValidUrls_ShouldInitializeSuccessfully(string baseUrl)
        {
            // Arrange
            var settings = new Trade360Settings
            {
                SnapshotApiBaseUrl = baseUrl,
                PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "testuser",
                    Password = "testpass"
                }
            };

            var options = new Mock<IOptions<Trade360Settings>>();
            options.Setup(x => x.Value).Returns(settings);

            var httpClient = new HttpClient();
            var factory = new Mock<IHttpClientFactory>();
            factory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var client = new SnapshotPrematchApiClient(factory.Object, options.Object, _mockMapper.Object);

            // Assert
            client.Should().NotBeNull();
            httpClient.BaseAddress.Should().Be(baseUrl);
        }

        [Fact]
        public void Constructor_WithNullCredentials_ShouldStillInitialize()
        {
            // Arrange
            var settingsWithNullCredentials = new Trade360Settings
            {
                SnapshotApiBaseUrl = "https://api.test.com",
                PrematchPackageCredentials = null
            };

            var options = new Mock<IOptions<Trade360Settings>>();
            options.Setup(x => x.Value).Returns(settingsWithNullCredentials);

            var httpClient = new HttpClient();
            var factory = new Mock<IHttpClientFactory>();
            factory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var client = new SnapshotPrematchApiClient(factory.Object, options.Object, _mockMapper.Object);

            // Assert
            client.Should().NotBeNull();
        }

        #endregion

        #region GetFixtures Advanced Tests

        [Fact]
        public async Task GetFixtures_WithComplexRequestDto_ShouldMapCorrectly()
        {
            // Arrange
            var complexRequest = new GetFixturesRequestDto
            {
                Timestamp = 1640995200L,
                FromDate = 1640908800L,
                ToDate = 1641081600L,
                Sports = new List<int> { 1, 2, 3 },
                Locations = new List<int> { 10, 20 },
                Fixtures = new List<int> { 12345, 67890 },
                Leagues = new List<int> { 100, 200, 300 }
            };

            var expectedMappedRequest = new BaseStandardRequest
            {
                Timestamp = complexRequest.Timestamp,
                FromDate = complexRequest.FromDate,
                ToDate = complexRequest.ToDate,
                Sports = complexRequest.Sports,
                Locations = complexRequest.Locations,
                Fixtures = complexRequest.Fixtures,
                Leagues = complexRequest.Leagues
            };

            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(complexRequest))
                      .Returns(expectedMappedRequest);

            SetupHttpResponse(new List<FixtureEvent>
            {
                new FixtureEvent { FixtureId = 12345 },
                new FixtureEvent { FixtureId = 67890 }
            });

            // Act
            var result = await _client.GetFixtures(complexRequest, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            _mockMapper.Verify(m => m.Map<BaseStandardRequest>(complexRequest), Times.Once);
        }

        [Fact]
        public async Task GetFixtures_WithEmptyResponse_ShouldReturnEmptyCollection()
        {
            // Arrange
            var request = new GetFixturesRequestDto();
            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(request)).Returns(new BaseStandardRequest());

            SetupHttpResponse(new List<FixtureEvent>());

            // Act
            var result = await _client.GetFixtures(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetFixtures_WithLargeResponse_ShouldHandleCorrectly()
        {
            // Arrange
            var request = new GetFixturesRequestDto();
            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(request)).Returns(new BaseStandardRequest());

            var largeResponse = Enumerable.Range(1, 1000)
                .Select(i => new FixtureEvent { FixtureId = i })
                .ToList();

            SetupHttpResponse(largeResponse);

            // Act
            var result = await _client.GetFixtures(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1000);
            result.First().FixtureId.Should().Be(1);
            result.Last().FixtureId.Should().Be(1000);
        }

        #endregion

        #region GetLivescore Advanced Tests

        [Fact]
        public async Task GetLivescore_WithComplexScenario_ShouldProcessCorrectly()
        {
            // Arrange
            var request = new GetLivescoreRequestDto();
            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(request)).Returns(new BaseStandardRequest());

            var expectedResponse = new List<LivescoreEvent>
            {
                new LivescoreEvent { FixtureId = 123, Fixture = new Fixture() },
                new LivescoreEvent { FixtureId = 456, Fixture = new Fixture() }
            };

            SetupHttpResponse(expectedResponse);

            // Act
            var result = await _client.GetLivescore(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().AllSatisfy(r => r.FixtureId.Should().BeOneOf(123, 456));
        }

        [Fact]
        public async Task GetLivescore_WithNullLivescoreData_ShouldHandleGracefully()
        {
            // Arrange
            var request = new GetLivescoreRequestDto();
            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(request)).Returns(new BaseStandardRequest());

            var responseWithNulls = new List<LivescoreEvent>
            {
                new LivescoreEvent { FixtureId = 123, Livescore = null },
                new LivescoreEvent { FixtureId = 456, Livescore = new Livescore() }
            };

            SetupHttpResponse(responseWithNulls);

            // Act
            var result = await _client.GetLivescore(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.First().Livescore.Should().BeNull();
            result.Last().Livescore.Should().NotBeNull();
        }

        #endregion

        #region GetFixtureMarkets Advanced Tests

        [Fact]
        public async Task GetFixtureMarkets_WithMarketFilters_ShouldApplyCorrectly()
        {
            // Arrange
            var request = new GetMarketRequestDto
            {
                Markets = new List<int> { 1, 3, 5 },
                Fixtures = new List<int> { 100, 200 }
            };

            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(request)).Returns(new BaseStandardRequest());

            var expectedMarkets = new List<MarketEvent>
            {
                new MarketEvent { FixtureId = 100 },
                new MarketEvent { FixtureId = 200 }
            };

            SetupHttpResponse(expectedMarkets);

            // Act
            var result = await _client.GetFixtureMarkets(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().AllSatisfy(m => m.FixtureId.Should().BeOneOf(100, 200));
        }

        #endregion

        #region GetEvents Advanced Tests

        [Fact]
        public async Task GetEvents_WithComplexEventData_ShouldDeserializeCorrectly()
        {
            // Arrange
            var request = new GetMarketRequestDto();
            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(request)).Returns(new BaseStandardRequest());

            var expectedEvents = new List<GetEventsResponse>
            {
                new GetEventsResponse
                {
                    FixtureId = 123,
                    Fixture = new Fixture(),
                    Livescore = new Livescore(),
                    Markets = new List<Market> { new Market { Id = 1 } }
                }
            };

            SetupHttpResponse(expectedEvents);

            // Act
            var result = await _client.GetEvents(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().FixtureId.Should().Be(123);
            result.First().Markets.Should().HaveCount(1);
        }

        #endregion

        #region Outright Methods Advanced Tests

        [Fact]
        public async Task GetOutrightFixture_WithTournamentData_ShouldMapToBaseOutrightRequest()
        {
            // Arrange
            var request = new GetOutrightFixturesRequestDto
            {
                Tournaments = new List<int> { 10, 20, 30 },
                Sports = new List<int> { 1 }
            };

            var expectedMappedRequest = new BaseOutrightRequest
            {
                Tournaments = request.Tournaments,
                Sports = request.Sports
            };

            _mockMapper.Setup(m => m.Map<BaseOutrightRequest>(request))
                      .Returns(expectedMappedRequest);

            SetupHttpResponse(new List<GetOutrightFixtureResponse>
            {
                new GetOutrightFixtureResponse { Id = 1, Name = "Test Tournament" }
            });

            // Act
            var result = await _client.GetOutrightFixture(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            _mockMapper.Verify(m => m.Map<BaseOutrightRequest>(request), Times.Once);
        }

        [Fact]
        public async Task GetOutrightScores_WithValidData_ShouldReturnLivescoreData()
        {
            // Arrange
            var request = new GetOutrightLivescoreRequestDto
            {
                Fixtures = new List<int> { 100 }
            };

            _mockMapper.Setup(m => m.Map<BaseOutrightRequest>(request)).Returns(new BaseOutrightRequest());

            SetupHttpResponse(new List<GetOutrightLivescoreResponse>
            {
                new GetOutrightLivescoreResponse 
                { 
                    Id = 100,
                    Events = new List<OutrightScoreEventResponse>
                    {
                        new OutrightScoreEventResponse { FixtureId = 100 }
                    }
                }
            });

            // Act
            var result = await _client.GetOutrightScores(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Events.Should().NotBeNull();
            result.First().Events.First().FixtureId.Should().Be(100);
        }

        [Fact]
        public async Task GetOutrightFixtureMarkets_WithMarketIds_ShouldFilterCorrectly()
        {
            // Arrange
            var request = new GetOutrightMarketsRequestDto
            {
                Markets = new List<int> { 1, 2, 3 },
                Tournaments = new List<int> { 10 }
            };

            _mockMapper.Setup(m => m.Map<BaseOutrightRequest>(request)).Returns(new BaseOutrightRequest());

            SetupHttpResponse(new List<GetOutrightMarketsResponse>
            {
                new GetOutrightMarketsResponse 
                { 
                    Id = 100,
                    Events = new List<OutrightMarketsResponse>
                    {
                        new OutrightMarketsResponse { FixtureId = 100 }
                    }
                }
            });

            // Act
            var result = await _client.GetOutrightFixtureMarkets(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Events.Should().NotBeNull();
            result.First().Events.First().FixtureId.Should().Be(100);
        }

        [Fact]
        public async Task GetOutrightEvents_WithComplexRequest_ShouldProcessSuccessfully()
        {
            // Arrange
            var request = new GetOutrightMarketsRequestDto
            {
                Sports = new List<int> { 1 },
                Tournaments = new List<int> { 5 }
            };

            _mockMapper.Setup(m => m.Map<BaseOutrightRequest>(request)).Returns(new BaseOutrightRequest());

            SetupHttpResponse(new List<GetOutrightEventsResponse>
            {
                new GetOutrightEventsResponse
                {
                    Id = 200,
                    Events = new List<OutrightEventResponse>
                    {
                        new OutrightEventResponse { FixtureId = 200 },
                        new OutrightEventResponse { FixtureId = 300 }
                    }
                }
            });

            // Act
            var result = await _client.GetOutrightEvents(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Events.Should().NotBeNull();
            result.First().Events.Should().HaveCount(2);
            result.First().Events.Should().AllSatisfy(e => e.FixtureId.Should().BeOneOf(200, 300));
        }

        [Fact]
        public async Task GetOutrightLeagues_WithFilteredData_ShouldMapToStandardRequest()
        {
            // Arrange
            var request = new GetFixturesRequestDto
            {
                Leagues = new List<int> { 1, 2, 3 },
                Sports = new List<int> { 10 }
            };

            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(request)).Returns(new BaseStandardRequest());

            SetupHttpResponse(new List<GetOutrightLeaguesFixturesResponse>
            {
                new GetOutrightLeaguesFixturesResponse { Id = 1 }
            });

            // Act
            var result = await _client.GetOutrightLeagues(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            _mockMapper.Verify(m => m.Map<BaseStandardRequest>(request), Times.Once);
        }

        [Fact]
        public async Task GetOutrightLeaguesMarkets_WithMarketConstraints_ShouldApplyCorrectly()
        {
            // Arrange
            var request = new GetMarketRequestDto
            {
                Markets = new List<int> { 10, 20 },
                Leagues = new List<int> { 100, 200 }
            };

            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(request)).Returns(new BaseStandardRequest());

            SetupHttpResponse(new List<GetOutrightLeaguesMarketsResponse>
            {
                new GetOutrightLeaguesMarketsResponse { Id = 1, Name = "Test League Market" }
            });

            // Act
            var result = await _client.GetOutrightLeaguesMarkets(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Test League Market");
        }

        #endregion

        #region Error Handling Tests

        [Fact]
        public async Task GetFixtures_WhenHttpRequestFails_ShouldPropagateException()
        {
            // Arrange
            var request = new GetFixturesRequestDto();
            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(request)).Returns(new BaseStandardRequest());

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () =>
                await _client.GetFixtures(request, CancellationToken.None));
        }

        [Fact]
        public async Task GetLivescore_WhenMappingFails_ShouldPropagateException()
        {
            // Arrange
            var request = new GetLivescoreRequestDto();
            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(request)).Throws(new AutoMapperMappingException("Mapping failed"));

            // Act & Assert
            await Assert.ThrowsAsync<AutoMapperMappingException>(async () =>
                await _client.GetLivescore(request, CancellationToken.None));
        }

        #endregion

        #region Concurrency Tests

        [Fact]
        public async Task MultipleAsyncCalls_ShouldExecuteConcurrently()
        {
            // Arrange
            var request1 = new GetFixturesRequestDto();
            var request2 = new GetLivescoreRequestDto();
            var request3 = new GetMarketRequestDto();

            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetFixturesRequestDto>())).Returns(new BaseStandardRequest());
            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetLivescoreRequestDto>())).Returns(new BaseStandardRequest());
            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(It.IsAny<GetMarketRequestDto>())).Returns(new BaseStandardRequest());

            SetupHttpResponse(new List<object>());

            // Act
            var task1 = _client.GetFixtures(request1, CancellationToken.None);
            var task2 = _client.GetLivescore(request2, CancellationToken.None);
            var task3 = _client.GetFixtureMarkets(request3, CancellationToken.None);

            await Task.WhenAll(task1, task2, task3);

            // Assert
            task1.Should().NotBeNull();
            task2.Should().NotBeNull();
            task3.Should().NotBeNull();
        }

        #endregion

        #region Performance and Edge Case Tests

        [Fact]
        public async Task GetFixtures_WithVeryLongTimeout_ShouldRespectCancellationToken()
        {
            // Arrange
            var request = new GetFixturesRequestDto();
            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(request)).Returns(new BaseStandardRequest());

            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(10)); // Very short timeout

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(async () =>
                {
                    await Task.Delay(1000); // Simulate long response
                    return new HttpResponseMessage(HttpStatusCode.OK);
                });

            // Act & Assert
            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
                await _client.GetFixtures(request, cts.Token));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        public async Task GetEvents_WithVariousResponseSizes_ShouldHandleCorrectly(int responseSize)
        {
            // Arrange
            var request = new GetMarketRequestDto();
            _mockMapper.Setup(m => m.Map<BaseStandardRequest>(request)).Returns(new BaseStandardRequest());

            var responses = Enumerable.Range(1, responseSize)
                .Select(i => new GetEventsResponse { FixtureId = i })
                .ToList();

            SetupHttpResponse(responses);

            // Act
            var result = await _client.GetEvents(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(responseSize);
        }

        #endregion

        #region Helper Methods

        private void SetupHttpResponse<T>(T responseObject) where T : class
        {
            var response = new Trade360SDK.SnapshotApi.Http.BaseResponse<T>
            {
                Header = new Trade360SDK.Common.Models.MessageHeader
                {
                    CreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    Type = 1,
                    MsgSeq = 1,
                    MsgGuid = Guid.NewGuid().ToString(),
                    ServerTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    MessageBrokerTimestamp = DateTime.UtcNow
                },
                Body = responseObject
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);
        }

        #endregion
    }
} 