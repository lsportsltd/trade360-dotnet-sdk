using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using AutoMapper;

namespace Trade360SDK.CustomersApi.Tests;

public class PackageDistributionHttpClientComprehensiveTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly PackageDistributionHttpClient _client;

    public PackageDistributionHttpClientComprehensiveTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        
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
        
        _client = new PackageDistributionHttpClient(mockHttpClientFactory.Object, settings.CustomersApiBaseUrl, packageCredentials);
    }

    private void SetupDefaultHttpResponse()
    {
        var defaultResponse = new GetDistributionStatusResponse
        {
            IsDistributionOn = false,
            Consumers = new string[0],
            NumberMessagesInQueue = 0,
            MessagesPerSecond = 0.0
        };

        var baseResponse = new Trade360SDK.CustomersApi.Entities.Base.BaseResponse<GetDistributionStatusResponse>
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
            Content = new StringContent(json, Encoding.UTF8)
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

    [Fact]
    public async Task GetDistributionStatusAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new GetDistributionStatusResponse
        {
            IsDistributionOn = true,
            Consumers = new[] { "consumer1", "consumer2" },
            NumberMessagesInQueue = 100,
            MessagesPerSecond = 50.5
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.GetDistributionStatusAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.IsDistributionOn.Should().BeTrue();
        result.Consumers.Should().HaveCount(2);
        result.NumberMessagesInQueue.Should().Be(100);
        result.MessagesPerSecond.Should().Be(50.5);
    }

    [Fact]
    public async Task StartDistributionAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new StartDistributionResponse
        {
            Message = "Distribution started successfully"
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.StartDistributionAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.Message.Should().Be("Distribution started successfully");
    }

    [Fact]
    public async Task StopDistributionAsync_WithValidRequest_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new StopDistributionResponse
        {
            Message = "Distribution stopped successfully"
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.StopDistributionAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.Message.Should().Be("Distribution stopped successfully");
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task GetDistributionStatusAsync_WithHttpErrors_ShouldThrowTrade360Exception(HttpStatusCode statusCode)
    {
        var errorResponse = new GetDistributionStatusResponse
        {
            IsDistributionOn = false,
            Consumers = new string[0],
            NumberMessagesInQueue = 0,
            MessagesPerSecond = 0.0
        };

        SetupHttpResponse(errorResponse, statusCode);

        var act = async () => await _client.GetDistributionStatusAsync(CancellationToken.None);

        await act.Should().ThrowAsync<Trade360SDK.Common.Exceptions.Trade360Exception>();
    }

    [Fact]
    public async Task StartDistributionAsync_WithForceRestart_ShouldReturnExpectedResponse()
    {
        var expectedResponse = new StartDistributionResponse
        {
            Message = "Distribution restarted successfully"
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.StartDistributionAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.Message.Should().Be("Distribution restarted successfully");
    }

    [Fact]
    public async Task StopDistributionAsync_WithFailure_ShouldReturnFailureResponse()
    {
        var expectedResponse = new StopDistributionResponse
        {
            Message = "Distribution could not be stopped"
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.StopDistributionAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.Message.Should().Be("Distribution could not be stopped");
    }

    [Fact]
    public async Task GetDistributionStatusAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        var act = async () => await _client.GetDistributionStatusAsync(cancellationTokenSource.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task StartDistributionAsync_WithInvalidPackageId_ShouldThrowTrade360Exception()
    {
        var expectedResponse = new StartDistributionResponse
        {
            Message = "Invalid package ID"
        };

        SetupHttpResponse(expectedResponse, HttpStatusCode.BadRequest);

        var act = async () => await _client.StartDistributionAsync(CancellationToken.None);

        await act.Should().ThrowAsync<Trade360SDK.Common.Exceptions.Trade360Exception>();
    }

    [Fact]
    public async Task StopDistributionAsync_WithAlreadyStoppedDistribution_ShouldReturnWarningResponse()
    {
        var expectedResponse = new StopDistributionResponse
        {
            Message = "Distribution was already stopped"
        };

        SetupHttpResponse(expectedResponse);

        var result = await _client.StopDistributionAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.Message.Should().Be("Distribution was already stopped");
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
            Content = new StringContent(json, Encoding.UTF8)
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
