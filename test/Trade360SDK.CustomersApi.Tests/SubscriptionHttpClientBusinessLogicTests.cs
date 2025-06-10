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
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using AutoMapper;

namespace Trade360SDK.CustomersApi.Tests;

public class SubscriptionHttpClientBusinessLogicTests
{
    private readonly SubscriptionHttpClient _client;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IMapper> _mockMapper;

    public SubscriptionHttpClientBusinessLogicTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockLogger = new Mock<ILogger<SubscriptionHttpClient>>();
        _mockMapper = new Mock<IMapper>();
        
        var settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/"
        };

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(settings.CustomersApiBaseUrl)
        };

        mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        
        var packageCredentials = new PackageCredentials
        {
            Username = "test_user",
            Password = "test_password",
            PackageId = 123
        };

        SetupDefaultHttpResponse();
        SetupAutoMapperDefaults();
        
        _client = new SubscriptionHttpClient(mockHttpClientFactory.Object, settings.CustomersApiBaseUrl, packageCredentials, _mockMapper.Object);
    }

    private void SetupDefaultHttpResponse()
    {
        var defaultResponse = new PackageQuotaResponse
        {
            CreditLimit = 1000,
            CreditRemaining = 900,
            UsedCredit = 100,
            CurrentPeriodStartDate = DateTime.UtcNow.AddDays(-30),
            CurrentPeriodEndDate = DateTime.UtcNow.AddDays(30)
        };

        var baseResponse = new Trade360SDK.CustomersApi.Entities.Base.BaseResponse<PackageQuotaResponse>
        {
            Header = new Trade360SDK.CustomersApi.Entities.Base.HeaderResponse
            {
                RequestId = Guid.NewGuid().ToString(),
                HttpStatusCode = HttpStatusCode.OK,
                Errors = new List<Trade360SDK.CustomersApi.Entities.Base.Error>()
            },
            Body = defaultResponse
        };

        var json = JsonSerializer.Serialize(baseResponse);
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
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

    private void SetupAutoMapperDefaults()
    {
        _mockMapper.Setup(x => x.Map<FixtureSubscriptionRequest>(It.IsAny<FixtureSubscriptionRequestDto>()))
            .Returns((FixtureSubscriptionRequestDto dto) => new FixtureSubscriptionRequest 
            { 
                Fixtures = dto?.Fixtures ?? new List<int>()
            });

        _mockMapper.Setup(x => x.Map<LeagueSubscriptionRequest>(It.IsAny<LeagueSubscriptionRequestDto>()))
            .Returns((LeagueSubscriptionRequestDto dto) => new LeagueSubscriptionRequest 
            { 
                Subscriptions = dto?.Subscriptions ?? new List<LeagueSubscription>()
            });

        _mockMapper.Setup(x => x.Map<CompetitionSubscriptionRequest>(It.IsAny<CompetitionSubscriptionRequestDto>()))
            .Returns((CompetitionSubscriptionRequestDto dto) => new CompetitionSubscriptionRequest 
            { 
                Subscriptions = dto?.Subscriptions ?? new List<CompetitionSubscription>()
            });

        _mockMapper.Setup(x => x.Map<GetSubscriptionRequest>(It.IsAny<GetSubscriptionRequestDto>()))
            .Returns((GetSubscriptionRequestDto dto) => new GetSubscriptionRequest());

        _mockMapper.Setup(x => x.Map<ChangeManualSuspensionRequest>(It.IsAny<ChangeManualSuspensionRequestDto>()))
            .Returns((ChangeManualSuspensionRequestDto dto) => new ChangeManualSuspensionRequest 
            { 
                Suspensions = dto?.Suspensions ?? new List<Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests.Suspension>()
            });

        _mockMapper.Setup(x => x.Map<GetFixtureMetadataRequest>(It.IsAny<GetFixtureMetadataRequestDto>()))
            .Returns((GetFixtureMetadataRequestDto dto) => new GetFixtureMetadataRequest 
            { 
                FromDate = dto != null ? dto.FromDate.ToString("yyyy-MM-ddTHH:mm:ss") : DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"),
                ToDate = dto != null ? dto.ToDate.ToString("yyyy-MM-ddTHH:mm:ss") : DateTime.UtcNow.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss")
            });

        _mockMapper.Setup(x => x.Map<GetFixtureScheduleRequest>(It.IsAny<GetFixtureScheduleRequestDto>()))
            .Returns((GetFixtureScheduleRequestDto dto) => new GetFixtureScheduleRequest());
    }

    [Fact]
    public async Task GetPackageQuotaAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new PackageQuotaResponse
        {
            CreditRemaining = 750,
            CreditLimit = 1000,
            UsedCredit = 250,
            CurrentPeriodStartDate = DateTime.UtcNow.AddDays(-30),
            CurrentPeriodEndDate = DateTime.UtcNow.AddDays(30)
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.GetPackageQuotaAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.CreditRemaining.Should().Be(750);
        result.CreditLimit.Should().Be(1000);
        result.UsedCredit.Should().Be(250);
    }

    [Fact]
    public async Task GetInplayFixtureSchedule_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new FixtureScheduleCollectionResponse
        {
            Fixtures = new List<FixtureSchedule> 
            { 
                new FixtureSchedule { FixtureId = 1, StartDate = DateTime.UtcNow.AddHours(2) },
                new FixtureSchedule { FixtureId = 2, StartDate = DateTime.UtcNow.AddHours(4) }
            }
        };

        SetupHttpResponse(expectedResponse);

        var requestDto = new GetFixtureScheduleRequestDto();
        var result = await _client.GetInplayFixtureSchedule(requestDto, CancellationToken.None);

        result.Should().NotBeNull();
        result.Fixtures.Should().HaveCount(2);
        result.Fixtures!.First().FixtureId.Should().Be(1);
    }

    [Fact]
    public async Task SubscribeByFixture_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new FixtureSubscriptionCollectionResponse
        {
            Fixtures = new List<FixtureSubscription> 
            { 
                new FixtureSubscription { FixtureId = 123, Success = true }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new FixtureSubscriptionRequestDto 
        { 
            Fixtures = new List<int> { 123, 456 }
        };

        var result = await _client.SubscribeByFixture(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Fixtures.Should().HaveCount(1);
        result.Fixtures.First().Success.Should().BeTrue();
    }

    [Fact]
    public async Task UnSubscribeByFixture_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new FixtureSubscriptionCollectionResponse
        {
            Fixtures = new List<FixtureSubscription> 
            { 
                new FixtureSubscription { FixtureId = 123, Success = false }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new FixtureSubscriptionRequestDto 
        { 
            Fixtures = new List<int> { 123 }
        };

        var result = await _client.UnSubscribeByFixture(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Fixtures.Should().HaveCount(1);
        result.Fixtures.First().Success.Should().BeFalse();
    }

    [Fact]
    public async Task SubscribeByLeague_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new LeagueSubscriptionCollectionResponse
        {
            Subscription = new List<LeagueSubscriptionResponse> 
            { 
                new LeagueSubscriptionResponse { LeagueId = 456, SportId = 1, Success = true }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new LeagueSubscriptionRequestDto 
        { 
            Subscriptions = new List<LeagueSubscription> { new LeagueSubscription { LeagueId = 456, SportId = 1, LocationId = 1 } }
        };

        var result = await _client.SubscribeByLeague(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Subscription.Should().HaveCount(1);
        result.Subscription.First().Success.Should().BeTrue();
    }

    [Fact]
    public async Task UnSubscribeByLeague_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new LeagueSubscriptionCollectionResponse
        {
            Subscription = new List<LeagueSubscriptionResponse> 
            { 
                new LeagueSubscriptionResponse { LeagueId = 456, SportId = 1, Success = true }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new LeagueSubscriptionRequestDto 
        { 
            Subscriptions = new List<LeagueSubscription> { new LeagueSubscription { LeagueId = 456, SportId = 1, LocationId = 1 } }
        };

        var result = await _client.UnSubscribeByLeague(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Subscription.Should().HaveCount(1);
        result.Subscription.First().Success.Should().BeTrue();
    }

    [Fact]
    public async Task GetSubscriptions_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new GetSubscriptionResponse
        {
            Fixtures = new List<int> { 123, 456 }
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetSubscriptionRequestDto();

        var result = await _client.GetSubscriptions(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Fixtures.Should().HaveCount(2);
        result.Fixtures.Should().Contain(123);
        result.Fixtures.Should().Contain(456);
    }

    [Fact]
    public async Task SubscribeByCompetition_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new CompetitionSubscriptionCollectionResponse
        {
            Subscription = new List<CompetitionSubscriptionResponse> 
            { 
                new CompetitionSubscriptionResponse { LeagueId = 789, SportId = 1, Success = true }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new CompetitionSubscriptionRequestDto 
        { 
            Subscriptions = new List<CompetitionSubscription> { new CompetitionSubscription { LeagueId = 789, SportId = 1, LocationId = 1 } }
        };

        var result = await _client.SubscribeByCompetition(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Subscription.Should().HaveCount(1);
        result.Subscription.First().Success.Should().BeTrue();
    }

    [Fact]
    public async Task UnSubscribeByCompetition_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new CompetitionSubscriptionCollectionResponse
        {
            Subscription = new List<CompetitionSubscriptionResponse> 
            { 
                new CompetitionSubscriptionResponse { LeagueId = 789, SportId = 1, Success = true }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new CompetitionSubscriptionRequestDto 
        { 
            Subscriptions = new List<CompetitionSubscription> { new CompetitionSubscription { LeagueId = 789, SportId = 1, LocationId = 1 } }
        };

        var result = await _client.UnSubscribeByCompetition(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Subscription.Should().HaveCount(1);
        result.Subscription.First().Success.Should().BeTrue();
    }

    [Fact]
    public async Task GetAllManualSuspensions_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new GetManualSuspensionResponse
        {
            Succeeded = true,
            Suspensions = new List<Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses.Suspension> 
            { 
                new Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses.Suspension { FixtureId = 123, Succeeded = true, Reason = "Technical issues", SportId = 1, CompetitionId = 100, CreationDate = DateTime.UtcNow },
                new Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses.Suspension { FixtureId = 456, Succeeded = false, Reason = null, SportId = 2, CompetitionId = 200, CreationDate = DateTime.UtcNow }
            },
            Reason = "Retrieved successfully"
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.GetAllManualSuspensions(CancellationToken.None);

        result.Should().NotBeNull();
        result.Succeeded.Should().BeTrue();
        result.Suspensions.Should().HaveCount(2);
        result.Suspensions.First().Succeeded.Should().BeTrue();
        result.Suspensions.First().Reason.Should().Be("Technical issues");
    }

    [Fact]
    public async Task AddManualSuspension_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new ChangeManualSuspensionResponse
        {
            Suspensions = new List<SuspensionChangeResponse> 
            { 
                new SuspensionChangeResponse 
                { 
                    FixtureId = 123,
                    Succeeded = true,
                    Reason = "Suspension added successfully",
                    SportId = 1,
                    CompetitionId = 100,
                    CreationDate = DateTime.UtcNow
                }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new ChangeManualSuspensionRequestDto 
        { 
            Suspensions = new List<Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests.Suspension>
            {
                new Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests.Suspension { FixtureId = 123 }
            }
        };

        var result = await _client.AddManualSuspension(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Suspensions.Should().HaveCount(1);
        result.Suspensions.First().Succeeded.Should().BeTrue();
        result.Suspensions.First().Reason.Should().Be("Suspension added successfully");
    }

    [Fact]
    public async Task RemoveManualSuspension_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new ChangeManualSuspensionResponse
        {
            Suspensions = new List<SuspensionChangeResponse> 
            { 
                new SuspensionChangeResponse 
                { 
                    FixtureId = 123,
                    Succeeded = true,
                    Reason = "Suspension removed successfully",
                    SportId = 1,
                    CompetitionId = 100,
                    CreationDate = DateTime.UtcNow
                }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new ChangeManualSuspensionRequestDto 
        { 
            Suspensions = new List<Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests.Suspension>
            {
                new Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests.Suspension { FixtureId = 123 }
            }
        };

        var result = await _client.RemoveManualSuspension(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Suspensions.Should().HaveCount(1);
        result.Suspensions.First().Succeeded.Should().BeTrue();
        result.Suspensions.First().Reason.Should().Be("Suspension removed successfully");
    }

    [Fact]
    public async Task GetFixtureMetadataAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new GetFixtureMetadataCollectionResponse
        {
            SubscribedFixtures = new List<SubscribedFixture> 
            { 
                new SubscribedFixture 
                { 
                    FixtureId = 123,
                    SportId = 1,
                    LeagueId = 10,
                    StartDate = DateTime.UtcNow.AddHours(2)
                }
            }
        };

        SetupHttpResponse(expectedResponse);

        var request = new GetFixtureMetadataRequestDto 
        { 
            FromDate = DateTime.UtcNow,
            ToDate = DateTime.UtcNow.AddDays(1)
        };

        var result = await _client.GetFixtureMetadataAsync(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.SubscribedFixtures.Should().HaveCount(1);
        result.SubscribedFixtures.First().FixtureId.Should().Be(123);
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetPackageQuotaAsync_WithHttpErrors_ShouldThrowTrade360Exception(HttpStatusCode statusCode)
    {
        var errorResponse = new PackageQuotaResponse
        {
            CreditRemaining = 0,
            CreditLimit = 1000,
            UsedCredit = 1000,
            CurrentPeriodStartDate = DateTime.UtcNow.AddDays(-30),
            CurrentPeriodEndDate = DateTime.UtcNow.AddDays(30)
        };

        SetupHttpResponse(errorResponse, statusCode);

        var act = async () => await _client.GetPackageQuotaAsync(CancellationToken.None);

        await act.Should().ThrowAsync<Trade360SDK.Common.Exceptions.Trade360Exception>();
    }

    [Fact]
    public async Task SubscribeByFixture_WithLargeFixtureList_ShouldHandleCorrectly()
    {
        var largeFixtureList = Enumerable.Range(1, 1000).ToList();
        var expectedResponse = new FixtureSubscriptionCollectionResponse
        {
            Fixtures = largeFixtureList.Select(id => new FixtureSubscription 
            { 
                FixtureId = id, 
                Success = true 
            }).ToList()
        };

        SetupHttpResponse(expectedResponse);

        var request = new FixtureSubscriptionRequestDto 
        { 
            Fixtures = largeFixtureList
        };

        var result = await _client.SubscribeByFixture(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Fixtures.Should().HaveCount(1000);
        result.Fixtures.All(s => s.Success).Should().BeTrue();
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
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post || req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse)
            .Verifiable();
    }
}
