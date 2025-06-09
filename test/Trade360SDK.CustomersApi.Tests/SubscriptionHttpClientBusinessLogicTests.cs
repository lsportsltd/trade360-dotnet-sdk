using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;

namespace Trade360SDK.CustomersApi.Tests;

public class SubscriptionHttpClientBusinessLogicTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<ILogger<SubscriptionHttpClient>> _mockLogger;
    private readonly Trade360Settings _settings;
    private readonly SubscriptionHttpClient _client;

    public SubscriptionHttpClientBusinessLogicTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockLogger = new Mock<ILogger<SubscriptionHttpClient>>();
        
        _settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/"
        };

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.CustomersApiBaseUrl)
        };

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        _client = new SubscriptionHttpClient(_mockHttpClientFactory.Object, _settings, _mockLogger.Object);
    }

    [Fact]
    public async Task GetPackageQuotaAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new PackageQuotaResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new PackageQuota 
            { 
                PackageId = 123,
                TotalQuota = 1000,
                UsedQuota = 250,
                RemainingQuota = 750
            }
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.GetPackageQuotaAsync();

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.PackageId.Should().Be(123);
        result.Body.RemainingQuota.Should().Be(750);
    }

    [Fact]
    public async Task GetInplayFixtureSchedule_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new FixtureScheduleCollectionResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<FixtureSchedule> 
            { 
                new FixtureSchedule { FixtureId = 1, StartDate = DateTime.UtcNow.AddHours(2) },
                new FixtureSchedule { FixtureId = 2, StartDate = DateTime.UtcNow.AddHours(4) }
            }
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.GetInplayFixtureSchedule();

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(2);
        result.Body.First().FixtureId.Should().Be(1);
    }

    [Fact]
    public async Task SubscribeByFixture_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new FixtureSubscribtionCollectionResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<FixtureSubscription> 
            { 
                new FixtureSubscription { FixtureId = 123, IsSubscribed = true }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new FixtureSubscriptionRequest 
        { 
            Fixtures = new List<int> { 123, 456 }
        };

        var result = await _client.SubscribeByFixture(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(1);
        result.Body.First().IsSubscribed.Should().BeTrue();
    }

    [Fact]
    public async Task UnSubscribeByFixture_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new FixtureSubscribtionCollectionResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<FixtureSubscription> 
            { 
                new FixtureSubscription { FixtureId = 123, IsSubscribed = false }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new FixtureSubscriptionRequest 
        { 
            Fixtures = new List<int> { 123 }
        };

        var result = await _client.UnSubscribeByFixture(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(1);
        result.Body.First().IsSubscribed.Should().BeFalse();
    }

    [Fact]
    public async Task SubscribeByLeague_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new LeagueSubscribtionCollectionResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<LeagueSubscription> 
            { 
                new LeagueSubscription { LeagueId = 456, IsSubscribed = true }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new LeagueSubscriptionRequest 
        { 
            Leagues = new List<int> { 456, 789 }
        };

        var result = await _client.SubscribeByLeague(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(1);
        result.Body.First().IsSubscribed.Should().BeTrue();
    }

    [Fact]
    public async Task UnSubscribeByLeague_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new LeagueSubscribtionCollectionResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<LeagueSubscription> 
            { 
                new LeagueSubscription { LeagueId = 456, IsSubscribed = false }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new LeagueSubscriptionRequest 
        { 
            Leagues = new List<int> { 456 }
        };

        var result = await _client.UnSubscribeByLeague(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(1);
        result.Body.First().IsSubscribed.Should().BeFalse();
    }

    [Fact]
    public async Task GetSubscriptions_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new GetSubscriptionResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new SubscriptionData
            {
                FixtureSubscriptions = new List<FixtureSubscription> 
                { 
                    new FixtureSubscription { FixtureId = 123, IsSubscribed = true }
                },
                LeagueSubscriptions = new List<LeagueSubscription> 
                { 
                    new LeagueSubscription { LeagueId = 456, IsSubscribed = true }
                }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetSubscriptionRequest 
        { 
            IncludeFixtures = true,
            IncludeLeagues = true
        };

        var result = await _client.GetSubscriptions(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.FixtureSubscriptions.Should().HaveCount(1);
        result.Body.LeagueSubscriptions.Should().HaveCount(1);
    }

    [Fact]
    public async Task SubscribeByCompetition_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new CompetitionSubscribtionCollectionResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<CompetitionSubscription> 
            { 
                new CompetitionSubscription { CompetitionId = 789, IsSubscribed = true }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new CompetitionSubscriptionRequest 
        { 
            Competitions = new List<int> { 789 }
        };

        var result = await _client.SubscribeByCompetition(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(1);
        result.Body.First().IsSubscribed.Should().BeTrue();
    }

    [Fact]
    public async Task UnSubscribeByCompetition_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new CompetitionSubscribtionCollectionResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<CompetitionSubscription> 
            { 
                new CompetitionSubscription { CompetitionId = 789, IsSubscribed = false }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new CompetitionSubscriptionRequest 
        { 
            Competitions = new List<int> { 789 }
        };

        var result = await _client.UnSubscribeByCompetition(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(1);
        result.Body.First().IsSubscribed.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllManualSuspensions_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new GetManualSuspensionResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<ManualSuspension> 
            { 
                new ManualSuspension { FixtureId = 123, IsSuspended = true, Reason = "Technical issues" },
                new ManualSuspension { FixtureId = 456, IsSuspended = false, Reason = null }
            }
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.GetAllManualSuspensions();

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(2);
        result.Body.First().IsSuspended.Should().BeTrue();
        result.Body.First().Reason.Should().Be("Technical issues");
    }

    [Fact]
    public async Task AddManualSuspension_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new ChangeManualSuspensionResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new ManualSuspensionResult 
            { 
                FixtureId = 123,
                Success = true,
                Message = "Suspension added successfully"
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new ChangeManualSuspensionRequest 
        { 
            FixtureId = 123,
            Reason = "Technical maintenance"
        };

        var result = await _client.AddManualSuspension(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Success.Should().BeTrue();
        result.Body.Message.Should().Be("Suspension added successfully");
    }

    [Fact]
    public async Task RemoveManualSuspension_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new ChangeManualSuspensionResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new ManualSuspensionResult 
            { 
                FixtureId = 123,
                Success = true,
                Message = "Suspension removed successfully"
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new ChangeManualSuspensionRequest 
        { 
            FixtureId = 123
        };

        var result = await _client.RemoveManualSuspension(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Success.Should().BeTrue();
        result.Body.Message.Should().Be("Suspension removed successfully");
    }

    [Fact]
    public async Task GetFixtureMetadataAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new GetFixtureMetadataCollectionResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = new List<FixtureMetadata> 
            { 
                new FixtureMetadata 
                { 
                    FixtureId = 123,
                    SportId = 1,
                    LeagueId = 10,
                    StartDate = DateTime.UtcNow.AddHours(2)
                }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetFixtureMetadataRequest 
        { 
            FromDate = DateTime.UtcNow,
            ToDate = DateTime.UtcNow.AddDays(1)
        };

        var result = await _client.GetFixtureMetadataAsync(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(1);
        result.Body.First().FixtureId.Should().Be(123);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetPackageQuotaAsync_WithHttpErrors_ShouldReturnErrorResponse(HttpStatusCode statusCode)
    {
        var errorResponse = new PackageQuotaResponse
        {
            Header = new HeaderResponse 
            { 
                HttpStatusCode = statusCode,
                Errors = new List<string> { $"HTTP {(int)statusCode} error occurred" }
            }
        };

        SetupHttpResponse(errorResponse, statusCode);

        var result = await _client.GetPackageQuotaAsync();

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(statusCode);
        result.Header.Errors.Should().Contain($"HTTP {(int)statusCode} error occurred");
    }

    [Fact]
    public async Task SubscribeByFixture_WithLargeFixtureList_ShouldHandleCorrectly()
    {
        var largeFixtureList = Enumerable.Range(1, 1000).ToList();
        var expectedResponse = new FixtureSubscribtionCollectionResponse
        {
            Header = new HeaderResponse { HttpStatusCode = HttpStatusCode.OK },
            Body = largeFixtureList.Select(id => new FixtureSubscription 
            { 
                FixtureId = id, 
                IsSubscribed = true 
            }).ToList()
        };

        SetupHttpResponse(expectedResponse);

        var request = new FixtureSubscriptionRequest 
        { 
            Fixtures = largeFixtureList
        };

        var result = await _client.SubscribeByFixture(request);

        result.Should().NotBeNull();
        result.Header.HttpStatusCode.Should().Be(HttpStatusCode.OK);
        result.Body.Should().HaveCount(1000);
        result.Body.All(s => s.IsSubscribed).Should().BeTrue();
    }

    private void SetupHttpResponse<T>(T responseObject, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        var json = JsonSerializer.Serialize(responseObject);
        var httpResponse = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);
    }
}
