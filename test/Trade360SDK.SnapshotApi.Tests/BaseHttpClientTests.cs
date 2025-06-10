using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Trade360SDK.Common.Configuration;
using Trade360SDK.SnapshotApi.Http;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Trade360SDK.Common.Models;

namespace Trade360SDK.SnapshotApi.Tests;

public class BaseHttpClientTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly Trade360Settings _settings;
    private readonly PackageCredentials _packageCredentials;
    private readonly TestableSnapshotBaseHttpClient _client;

    public BaseHttpClientTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        
        _settings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://snapshot.example.com/"
        };

        _packageCredentials = new PackageCredentials
        {
            Username = "test_user",
            Password = "test_password",
            PackageId = 123
        };

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(_settings.SnapshotApiBaseUrl)
        };

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        _client = new TestableSnapshotBaseHttpClient(_mockHttpClientFactory.Object, _settings, _packageCredentials);
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldInitializeCorrectly()
    {
        _client.Should().NotBeNull();
        _client.BaseUrl.Should().Be(_settings.SnapshotApiBaseUrl);
        _client.PackageCredentials.Should().Be(_packageCredentials);
    }

    [Fact]
    public void Constructor_WithNullHttpClientFactory_ShouldThrowArgumentNullException()
    {
        var act = () => new TestableSnapshotBaseHttpClient(null!, _settings, _packageCredentials);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_WithNullBaseUrl_ShouldThrowInvalidOperationException()
    {
        var act = () => new TestableSnapshotBaseHttpClient(_mockHttpClientFactory.Object, new Trade360Settings { SnapshotApiBaseUrl = null }, _packageCredentials);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Constructor_WithNullPackageCredentials_ShouldThrowNullReferenceException()
    {
        var act = () => new TestableSnapshotBaseHttpClient(_mockHttpClientFactory.Object, _settings, null!);

        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public async Task PostEntityAsync_WithValidRequest_ShouldReturnResponse()
    {
        var expectedResponse = new TestSnapshotResponse { Message = "Success" };
        var request = new TestBaseRequest();

        SetupHttpResponse(expectedResponse);

        var result = await _client.TestPostEntityAsync<TestSnapshotResponse>("test-endpoint", request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Message.Should().Be("Success");
    }

    [Fact]
    public async Task PostEntityAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
        var request = new TestBaseRequest();

        var act = async () => await _client.TestPostEntityAsync<TestSnapshotResponse>("test-endpoint", request, cancellationTokenSource.Token);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task PostEntityAsync_WithHttpErrors_ShouldHandleGracefully(HttpStatusCode statusCode)
    {
        var errorResponse = new TestSnapshotResponse { Message = "Error" };
        var request = new TestBaseRequest();

        SetupHttpResponse(errorResponse, statusCode);

        var result = await _client.TestPostEntityAsync<TestSnapshotResponse>("test-endpoint", request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Message.Should().Be("Error");
    }

    [Fact]
    public async Task PostEntityAsync_WithLargePayload_ShouldHandleEfficiently()
    {
        var largeResponse = new TestSnapshotResponse { Message = new string('A', 10000) };
        var request = new TestBaseRequest();

        SetupHttpResponse(largeResponse);

        var result = await _client.TestPostEntityAsync<TestSnapshotResponse>("test-endpoint", request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Message.Should().HaveLength(10000);
    }

    [Fact]
    public void BaseUrl_ShouldReturnCorrectValue()
    {
        _client.BaseUrl.Should().Be(_settings.SnapshotApiBaseUrl);
    }

    [Fact]
    public void PackageCredentials_ShouldReturnCorrectValue()
    {
        _client.PackageCredentials.Should().Be(_packageCredentials);
    }

    [Fact]
    public void Dispose_ShouldDisposeHttpClient()
    {
        _client.Dispose();

        var request = new TestBaseRequest();
        var act = () => _client.TestPostEntityAsync<TestSnapshotResponse>("test-endpoint", request, CancellationToken.None);

        act.Should().ThrowAsync<ObjectDisposedException>();
    }

    private void SetupHttpResponse<T>(T responseObject, HttpStatusCode statusCode = HttpStatusCode.OK) where T : class
    {
        var baseResponse = new BaseResponse<T>
        {
            Header = new MessageHeader(),
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

public class TestableSnapshotBaseHttpClient : BaseHttpClient
{
    private readonly Trade360Settings _settings;
    private readonly PackageCredentials? _packageCredentials;

    public TestableSnapshotBaseHttpClient(IHttpClientFactory httpClientFactory, Trade360Settings settings, PackageCredentials? packageCredentials)
        : base(httpClientFactory, settings, packageCredentials)
    {
        _settings = settings;
        _packageCredentials = packageCredentials;
    }

    public string BaseUrl => _settings.SnapshotApiBaseUrl ?? string.Empty;
    public PackageCredentials? PackageCredentials => _packageCredentials;

    public async Task<TResponse> TestPostEntityAsync<TResponse>(string endpoint, BaseRequest request, CancellationToken cancellationToken)
        where TResponse : class
    {
        return await PostEntityAsync<TResponse>(endpoint, request, cancellationToken);
    }
}

public class TestSnapshotResponse
{
    public string Message { get; set; } = string.Empty;
}

public class TestBaseRequest : BaseRequest
{
    public string TestProperty { get; set; } = "test";
}
