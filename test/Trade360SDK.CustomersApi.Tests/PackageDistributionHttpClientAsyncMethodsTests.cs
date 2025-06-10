using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using Trade360SDK.CustomersApi.Entities.Base;

namespace Trade360SDK.CustomersApi.Tests;

public class PackageDistributionHttpClientAsyncMethodsTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Mock<IOptions<Trade360Settings>> _mockOptions;
    private readonly Trade360Settings _settings;
    private readonly PackageDistributionHttpClient _client;

    public PackageDistributionHttpClientAsyncMethodsTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _mockOptions = new Mock<IOptions<Trade360Settings>>();
        
        _settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://customers.example.com/"
        };

        _mockOptions.Setup(x => x.Value).Returns(_settings);

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.CustomersApiBaseUrl)
        };

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var packageCredentials = new PackageCredentials
        {
            PackageId = 123,
            Username = "testuser",
            Password = "testpass"
        };
        
        _client = new PackageDistributionHttpClient(_mockHttpClientFactory.Object, _settings.CustomersApiBaseUrl, packageCredentials);
    }

    [Fact]
    public async Task GetDistributionStatusAsync_WithValidRequest_ShouldReturnStatus()
    {
        var expectedStatus = new GetDistributionStatusResponse
        {
            IsDistributionOn = true,
            Consumers = new[] { "consumer1", "consumer2" },
            NumberMessagesInQueue = 100,
            MessagesPerSecond = 50.5
        };

        SetupHttpResponse(expectedStatus);

        var result = await _client.GetDistributionStatusAsync(CancellationToken.None);

        result.Should().NotBeNull();
        result.IsDistributionOn.Should().BeTrue();
        result.Consumers.Should().NotBeNull();
        result.Consumers.Should().HaveCount(2);
        result.NumberMessagesInQueue.Should().Be(100);
        result.MessagesPerSecond.Should().Be(50.5);
    }

    [Fact]
    public async Task StartDistributionAsync_WithValidRequest_ShouldReturnStartResponse()
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
    public async Task StopDistributionAsync_WithValidRequest_ShouldReturnStopResponse()
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
    public async Task StartDistributionAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Returns((HttpRequestMessage request, CancellationToken token) =>
            {
                token.ThrowIfCancellationRequested();
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            });

        cancellationTokenSource.Cancel();

        var act = async () => await _client.StartDistributionAsync(cancellationTokenSource.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task StopDistributionAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .Returns((HttpRequestMessage request, CancellationToken token) =>
            {
                token.ThrowIfCancellationRequested();
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            });

        cancellationTokenSource.Cancel();

        var act = async () => await _client.StopDistributionAsync(cancellationTokenSource.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task GetDistributionStatusAsync_WithMultipleCalls_ShouldHandleConsistently()
    {
        var expectedStatus = new GetDistributionStatusResponse
        {
            IsDistributionOn = true,
            Consumers = new[] { "consumer1", "consumer2" },
            NumberMessagesInQueue = 100,
            MessagesPerSecond = 50.5
        };

        SetupHttpResponse(expectedStatus);

        var result1 = await _client.GetDistributionStatusAsync(CancellationToken.None);
        var result2 = await _client.GetDistributionStatusAsync(CancellationToken.None);

        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.IsDistributionOn.Should().Be(result2.IsDistributionOn);
        result1.NumberMessagesInQueue.Should().Be(result2.NumberMessagesInQueue);
    }

    private void SetupHttpResponse<T>(T responseObject, HttpStatusCode statusCode = HttpStatusCode.OK) where T : class
    {
        var baseResponse = new Trade360SDK.CustomersApi.Entities.Base.BaseResponse<T>
        {
            Header = new Trade360SDK.CustomersApi.Entities.Base.HeaderResponse
            {
                RequestId = Guid.NewGuid().ToString(),
                HttpStatusCode = statusCode
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
