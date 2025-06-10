using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Linq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using AutoMapper;

namespace Trade360SDK.CustomersApi.Tests;

public class SubscriptionHttpClientAsyncMethodsTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Trade360Settings _settings;
    private readonly SubscriptionHttpClient _client;

    public SubscriptionHttpClientAsyncMethodsTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        
        _settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/"
        };

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.CustomersApiBaseUrl)
        };

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        
        var packageCredentials = new PackageCredentials
        {
            Username = "test_user",
            Password = "test_password",
            PackageId = 123
        };

        var mockMapper = new Mock<AutoMapper.IMapper>();
        
        mockMapper.Setup(m => m.Map<GetFixtureScheduleRequest>(It.IsAny<GetFixtureScheduleRequestDto>()))
            .Returns((GetFixtureScheduleRequestDto _) => new GetFixtureScheduleRequest());
            
        mockMapper.Setup(m => m.Map<FixtureSubscriptionRequest>(It.IsAny<FixtureSubscriptionRequestDto>()))
            .Returns((FixtureSubscriptionRequestDto _) => new FixtureSubscriptionRequest());
            
        mockMapper.Setup(m => m.Map<LeagueSubscriptionRequest>(It.IsAny<LeagueSubscriptionRequestDto>()))
            .Returns((LeagueSubscriptionRequestDto _) => new LeagueSubscriptionRequest());
            
        mockMapper.Setup(m => m.Map<GetSubscriptionRequest>(It.IsAny<GetSubscriptionRequestDto>()))
            .Returns((GetSubscriptionRequestDto _) => new GetSubscriptionRequest());
            
        mockMapper.Setup(m => m.Map<CompetitionSubscriptionRequest>(It.IsAny<CompetitionSubscriptionRequestDto>()))
            .Returns((CompetitionSubscriptionRequestDto _) => new CompetitionSubscriptionRequest());
            
        mockMapper.Setup(m => m.Map<ChangeManualSuspensionRequest>(It.IsAny<ChangeManualSuspensionRequestDto>()))
            .Returns((ChangeManualSuspensionRequestDto _) => new ChangeManualSuspensionRequest());
            
        mockMapper.Setup(m => m.Map<GetFixtureMetadataRequest>(It.IsAny<GetFixtureMetadataRequestDto>()))
            .Returns((GetFixtureMetadataRequestDto _) => new GetFixtureMetadataRequest());
        
        _client = new SubscriptionHttpClient(_mockHttpClientFactory.Object, _settings.CustomersApiBaseUrl, packageCredentials, mockMapper.Object);
    }

    [Fact]
    public async Task GetPackageQuotaAsync_WithValidRequest_ShouldReturnQuota()
    {
        var expectedQuota = new PackageQuotaResponse
        {
            CreditLimit = 1000,
            UsedCredit = 250,
            CreditRemaining = 750,
            CurrentPeriodStartDate = DateTime.UtcNow.AddDays(-30),
            CurrentPeriodEndDate = DateTime.UtcNow
        };

        SetupHttpResponse(expectedQuota);

        var result = await _client.GetPackageQuotaAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().NotBeNull();
        result.UsedCredit.Should().Be(250);
        result.CreditRemaining.Should().Be(750);
    }

    [Fact]
    public async Task GetInplayFixtureSchedule_WithValidRequest_ShouldReturnSchedule()
    {
        var expectedSchedule = new FixtureScheduleCollectionResponse
        {
            Fixtures = new[]
            {
                new FixtureSchedule { FixtureId = 1, StartDate = DateTime.UtcNow },
                new FixtureSchedule { FixtureId = 2, StartDate = DateTime.UtcNow.AddHours(1) }
            }
        };

        SetupHttpResponse(expectedSchedule);

        var request = new GetFixtureScheduleRequestDto
        {
            SportIds = new[] { 1, 2 },
            LocationIds = new[] { 1 },
            LeagueIds = new[] { 1, 2 }
        };

        var result = await _client.GetInplayFixtureSchedule(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Fixtures.Should().HaveCount(2);
        result.Fixtures.First().FixtureId.Should().Be(1);
    }

    [Fact]
    public async Task SubscribeByFixture_WithValidRequest_ShouldReturnSuccess()
    {
        var expectedResponse = new FixtureSubscriptionCollectionResponse
        {
            Fixtures = new List<FixtureSubscription>
            {
                new FixtureSubscription { FixtureId = 12345, Success = true },
                new FixtureSubscription { FixtureId = 67890, Success = true }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new FixtureSubscriptionRequestDto
        {
            Fixtures = new[] { 12345, 67890 }
        };

        var result = await _client.SubscribeByFixture(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UnSubscribeByFixture_WithValidRequest_ShouldReturnSuccess()
    {
        var expectedResponse = new FixtureSubscriptionCollectionResponse
        {
            Fixtures = new List<FixtureSubscription>
            {
                new FixtureSubscription { FixtureId = 12345, Success = true },
                new FixtureSubscription { FixtureId = 67890, Success = true }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new FixtureSubscriptionRequestDto
        {
            Fixtures = new[] { 12345, 67890 }
        };

        var result = await _client.UnSubscribeByFixture(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task SubscribeByLeague_WithValidRequest_ShouldReturnSuccess()
    {
        var expectedResponse = new LeagueSubscriptionCollectionResponse
        {
            Subscription = new List<LeagueSubscriptionResponse>
            {
                new LeagueSubscriptionResponse { LeagueId = 100, Success = true, Message = "League subscription successful" },
                new LeagueSubscriptionResponse { LeagueId = 200, Success = true, Message = "League subscription successful" }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new LeagueSubscriptionRequestDto
        {
            Subscriptions = new List<LeagueSubscription>
            {
                new LeagueSubscription { LeagueId = 100, SportId = 1, LocationId = 1 },
                new LeagueSubscription { LeagueId = 200, SportId = 2, LocationId = 2 }
            }
        };

        var result = await _client.SubscribeByLeague(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UnSubscribeByLeague_WithValidRequest_ShouldReturnSuccess()
    {
        var expectedResponse = new LeagueSubscriptionCollectionResponse
        {
            Subscription = new List<LeagueSubscriptionResponse>
            {
                new LeagueSubscriptionResponse { LeagueId = 100, Success = true, Message = "League unsubscription successful" },
                new LeagueSubscriptionResponse { LeagueId = 200, Success = true, Message = "League unsubscription successful" }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new LeagueSubscriptionRequestDto
        {
            Subscriptions = new List<LeagueSubscription>
            {
                new LeagueSubscription { LeagueId = 100, SportId = 1, LocationId = 1 },
                new LeagueSubscription { LeagueId = 200, SportId = 2, LocationId = 2 }
            }
        };

        var result = await _client.UnSubscribeByLeague(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetSubscriptions_WithValidRequest_ShouldReturnSubscriptions()
    {
        var expectedSubscriptions = new GetSubscriptionResponse
        {
            Fixtures = new[] { 12345, 67890 }
        };

        SetupHttpResponse(expectedSubscriptions);

        var request = new GetSubscriptionRequestDto();
        var result = await _client.GetSubscriptions(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Fixtures.Should().HaveCount(2);
    }

    [Fact]
    public async Task SubscribeByCompetition_WithValidRequest_ShouldReturnSuccess()
    {
        var expectedResponse = new CompetitionSubscriptionCollectionResponse
        {
            Subscription = new List<CompetitionSubscriptionResponse>
            {
                new CompetitionSubscriptionResponse { LeagueId = 300, SportId = 3, LocationId = 3, Success = true },
                new CompetitionSubscriptionResponse { LeagueId = 400, SportId = 4, LocationId = 4, Success = true }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new CompetitionSubscriptionRequestDto
        {
            Subscriptions = new List<CompetitionSubscription>
            {
                new CompetitionSubscription { LeagueId = 300, SportId = 3, LocationId = 3 },
                new CompetitionSubscription { LeagueId = 400, SportId = 4, LocationId = 4 }
            }
        };

        var result = await _client.SubscribeByCompetition(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UnSubscribeByCompetition_WithValidRequest_ShouldReturnSuccess()
    {
        var expectedResponse = new CompetitionSubscriptionCollectionResponse
        {
            Subscription = new List<CompetitionSubscriptionResponse>
            {
                new CompetitionSubscriptionResponse { LeagueId = 300, SportId = 3, LocationId = 3, Success = true },
                new CompetitionSubscriptionResponse { LeagueId = 400, SportId = 4, LocationId = 4, Success = true }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new CompetitionSubscriptionRequestDto
        {
            Subscriptions = new List<CompetitionSubscription>
            {
                new CompetitionSubscription { LeagueId = 300, SportId = 3, LocationId = 3 },
                new CompetitionSubscription { LeagueId = 400, SportId = 4, LocationId = 4 }
            }
        };

        var result = await _client.UnSubscribeByCompetition(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllManualSuspensions_WithValidRequest_ShouldReturnSuspensions()
    {
        var expectedSuspensions = new GetManualSuspensionResponse
        {
            Succeeded = true,
            Suspensions = new List<Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses.Suspension>
            {
                new Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses.Suspension { SportId = 12345 },
                new Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses.Suspension { SportId = 67890 }
            }
        };

        SetupHttpResponse(expectedSuspensions);

        var result = await _client.GetAllManualSuspensions(CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Suspensions.Should().HaveCount(2);
    }

    [Fact]
    public async Task AddManualSuspension_WithValidRequest_ShouldReturnSuccess()
    {
        var expectedResponse = new ChangeManualSuspensionResponse
        {
            Suspensions = new List<SuspensionChangeResponse>
            {
                new SuspensionChangeResponse { Succeeded = true, Reason = "Manual suspension added successfully" }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new ChangeManualSuspensionRequestDto
        {
            Suspensions = new List<Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests.Suspension>
            {
                new Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests.Suspension { SportId = 12345 }
            }
        };

        var result = await _client.AddManualSuspension(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Suspensions.Should().NotBeNull();
        result.Suspensions.First().Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task RemoveManualSuspension_WithValidRequest_ShouldReturnSuccess()
    {
        var expectedResponse = new ChangeManualSuspensionResponse
        {
            Suspensions = new List<SuspensionChangeResponse>
            {
                new SuspensionChangeResponse { Succeeded = true, Reason = "Manual suspension removed successfully" }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new ChangeManualSuspensionRequestDto
        {
            Suspensions = new List<Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests.Suspension>
            {
                new Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests.Suspension { SportId = 12345 }
            }
        };

        var result = await _client.RemoveManualSuspension(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Suspensions.Should().NotBeNull();
        result.Suspensions.First().Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task GetFixtureMetadataAsync_WithValidRequest_ShouldReturnMetadata()
    {
        var expectedMetadata = new GetFixtureMetadataCollectionResponse
        {
            SubscribedFixtures = new[]
            {
                new SubscribedFixture { FixtureId = 12345, SportId = 1, LeagueId = 100 },
                new SubscribedFixture { FixtureId = 67890, SportId = 2, LeagueId = 200 }
            }
        };

        SetupHttpResponse(expectedMetadata);

        var request = new GetFixtureMetadataRequestDto
        {
            FromDate = DateTime.UtcNow,
            ToDate = DateTime.UtcNow.AddDays(1)
        };

        var result = await _client.GetFixtureMetadataAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.SubscribedFixtures.Should().HaveCount(2);
        result.SubscribedFixtures.First().FixtureId.Should().Be(12345);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetPackageQuotaAsync_WithHttpErrors_ShouldThrowTrade360Exception(HttpStatusCode statusCode)
    {
        var errorQuota = new PackageQuotaResponse
        {
            CreditLimit = 0,
            UsedCredit = 0,
            CreditRemaining = 0,
            CurrentPeriodStartDate = DateTime.UtcNow,
            CurrentPeriodEndDate = DateTime.UtcNow
        };

        SetupHttpResponse(errorQuota, statusCode);

        var act = async () => await _client.GetPackageQuotaAsync(CancellationToken.None);

        await act.Should().ThrowAsync<Trade360SDK.Common.Exceptions.Trade360Exception>();
    }

    [Fact]
    public async Task SubscribeByFixture_WithLargeFixtureList_ShouldHandleEfficiently()
    {
        var expectedResponse = new FixtureSubscriptionCollectionResponse
        {
            Fixtures = new List<FixtureSubscription>
            {
                new FixtureSubscription { FixtureId = 1, Success = true }
            }
        };

        SetupHttpResponse(expectedResponse);

        var largeFixtureList = Enumerable.Range(1, 1000).ToArray();
        var request = new FixtureSubscriptionRequestDto
        {
            Fixtures = largeFixtureList
        };

        var result = await _client.SubscribeByFixture(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetInplayFixtureSchedule_WithCancellationToken_ShouldRespectCancellation()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Returns((HttpRequestMessage _, CancellationToken token) =>
            {
                token.ThrowIfCancellationRequested();
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            });

        cancellationTokenSource.Cancel();

        var request = new GetFixtureScheduleRequestDto
        {
            SportIds = new[] { 1, 2 },
            LocationIds = new[] { 1 },
            LeagueIds = new[] { 1, 2 }
        };

        var act = async () => await _client.GetInplayFixtureSchedule(request, cancellationTokenSource.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    private void SetupHttpResponse<T>(T responseObject, HttpStatusCode statusCode = HttpStatusCode.OK) where T : class
    {
        var baseResponse = new Trade360SDK.CustomersApi.Entities.Base.BaseResponse<T>
        {
            Header = new Trade360SDK.CustomersApi.Entities.Base.HeaderResponse
            {
                RequestId = Guid.NewGuid().ToString(),
                HttpStatusCode = statusCode,
                Errors = new List<Trade360SDK.CustomersApi.Entities.Base.Error>()
            },
            Body = responseObject
        };

        var json = JsonSerializer.Serialize(baseResponse);
        var httpResponse = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
        httpResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse)
            .Verifiable();
    }


}
