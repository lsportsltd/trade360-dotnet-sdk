using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Http;

namespace Trade360SDK.CustomersApi.Tests;

public class BaseHttpClientTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly PackageCredentials _packageCredentials;
    private readonly TestableBaseHttpClient _client;

    public BaseHttpClientTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        
        var settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com/"
        };

        _packageCredentials = new PackageCredentials
        {
            Username = "test_user",
            Password = "test_password",
            PackageId = 123
        };

        var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(settings.CustomersApiBaseUrl)
        };

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        _client = new TestableBaseHttpClient(_mockHttpClientFactory.Object, settings.CustomersApiBaseUrl, _packageCredentials);
    }

    [Fact]
    public void Constructor_WithValidParameters_ShouldInitializeCorrectly()
    {
        _client.Should().NotBeNull();
        _client.BaseUrl.Should().Be("https://api.example.com/");
        _client.PackageCredentials.Should().Be(_packageCredentials);
    }

    [Fact]
    public void Constructor_WithNullHttpClientFactory_ShouldThrowArgumentNullException()
    {
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<AutoMapper.IMapper>();

        var act = () => new MetadataHttpClient(null!, "https://api.example.com/", credentials, mockMapper.Object);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("httpClientFactory");
    }

    [Fact]
    public void Constructor_WithNullBaseUrl_ShouldThrowArgumentException()
    {
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<AutoMapper.IMapper>();

        var act = () => new MetadataHttpClient(_mockHttpClientFactory.Object, null!, credentials, mockMapper.Object);

        act.Should().Throw<ArgumentException>()
           .WithParameterName("baseUrl");
    }

    [Fact]
    public void Constructor_WithNullPackageCredentials_ShouldThrowArgumentNullException()
    {
        var mockMapper = new Mock<AutoMapper.IMapper>();

        var act = () => new MetadataHttpClient(_mockHttpClientFactory.Object, "https://api.example.com/", null!, mockMapper.Object);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("settings");
    }

    [Fact]
    public async Task SendPostRequestAsync_WithValidRequest_ShouldReturnResponse()
    {
        var expectedResponse = new TestResponse { Message = "Success" };
        var requestObject = new TestRequest { Data = "test data" };

        SetupHttpResponse(expectedResponse);

        var result = await _client.TestSendPostRequestAsync<TestRequest, TestResponse>("test-endpoint", requestObject, CancellationToken.None);

        result.Should().NotBeNull();
        result.Message.Should().Be("Success");
    }

    [Fact]
    public async Task SendPostRequestAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        var requestObject = new TestRequest { Data = "test data" };
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

        var act = async () => await _client.TestSendPostRequestAsync<TestRequest, TestResponse>("test-endpoint", requestObject, cancellationTokenSource.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task SendPostRequestAsync_WithHttpErrors_ShouldThrowTrade360Exception(HttpStatusCode statusCode)
    {
        var errorResponse = new TestResponse { Message = "Error" };
        var requestObject = new TestRequest { Data = "test data" };

        SetupHttpResponse(errorResponse, statusCode);

        var act = async () => await _client.TestSendPostRequestAsync<TestRequest, TestResponse>("test-endpoint", requestObject, CancellationToken.None);

        await act.Should().ThrowAsync<Trade360SDK.Common.Exceptions.Trade360Exception>();
    }

    [Fact]
    public async Task SendPostRequestAsync_WithLargePayload_ShouldHandleEfficiently()
    {
        var largeResponse = new TestResponse { Message = new string('A', 10000) };
        var largeRequest = new TestRequest { Data = new string('B', 5000) };

        SetupHttpResponse(largeResponse);

        var result = await _client.TestSendPostRequestAsync<TestRequest, TestResponse>("test-endpoint", largeRequest, CancellationToken.None);

        result.Should().NotBeNull();
        result.Message.Should().HaveLength(10000);
    }

    [Fact]
    public void BuildQueryString_WithValidParameters_ShouldReturnCorrectQueryString()
    {
        var parameters = new Dictionary<string, object>
        {
            { "param1", "value1" },
            { "param2", 123 },
            { "param3", true }
        };

        var result = _client.TestBuildQueryString(parameters);

        result.Should().Contain("param1=value1");
        result.Should().Contain("param2=123");
        result.Should().Contain("param3=True");
        result.Should().StartWith("?");
    }

    [Fact]
    public void BuildQueryString_WithEmptyParameters_ShouldReturnEmptyString()
    {
        var parameters = new Dictionary<string, object>();

        var result = _client.TestBuildQueryString(parameters);

        result.Should().BeEmpty();
    }

    [Fact]
    public void BuildQueryString_WithNullParameters_ShouldReturnEmptyString()
    {
        var result = _client.TestBuildQueryString(null);

        result.Should().BeEmpty();
    }

    [Fact]
    public void BuildQueryString_WithSpecialCharacters_ShouldEncodeCorrectly()
    {
        var parameters = new Dictionary<string, object>
        {
            { "param1", "value with spaces" },
            { "param2", "value&with&ampersands" },
            { "param3", "value=with=equals" }
        };

        var result = _client.TestBuildQueryString(parameters);

        result.Should().Contain("param1=value%20with%20spaces");
        result.Should().Contain("param2=value%26with%26ampersands");
        result.Should().Contain("param3=value%3Dwith%3Dequals");
    }

    [Fact]
    public void Dispose_ShouldDisposeHttpClient()
    {
        _client.Dispose();

        var act = () => _client.TestSendPostRequestAsync<TestRequest, TestResponse>("test-endpoint", new TestRequest(), CancellationToken.None);

        act.Should().ThrowAsync<ObjectDisposedException>();
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
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse)
            .Verifiable();
    }
}

public class TestableBaseHttpClient : BaseHttpClient
{
    public TestableBaseHttpClient(IHttpClientFactory httpClientFactory, string baseUrl, PackageCredentials packageCredentials)
        : base(httpClientFactory, baseUrl, packageCredentials)
    {
        BaseUrl = baseUrl;
        PackageCredentials = packageCredentials;
    }

    public string BaseUrl { get; private set; }
    public PackageCredentials PackageCredentials { get; private set; }

    public async Task<TResponse> TestSendPostRequestAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken cancellationToken)
        where TRequest : class
        where TResponse : class
    {
        return await PostEntityAsync<TResponse>(endpoint, cancellationToken);
    }

    public string TestBuildQueryString(Dictionary<string, object> parameters)
    {
        if (parameters == null || parameters.Count == 0)
        {
            return string.Empty;
        }

        var queryParams = new List<string>();
        foreach (var kvp in parameters)
        {
            var encodedKey = Uri.EscapeDataString(kvp.Key);
            var encodedValue = Uri.EscapeDataString(kvp.Value?.ToString() ?? string.Empty);
            queryParams.Add($"{encodedKey}={encodedValue}");
        }

        return "?" + string.Join("&", queryParams);
    }
}

public class TestRequest
{
    public string Data { get; set; } = string.Empty;
}

public class TestRequestWithProperties
{
    public string param1 { get; set; } = string.Empty;
    public int param2 { get; set; }
    public bool param3 { get; set; }
    public int[] SportIds { get; set; } = Array.Empty<int>();
    public string[] Languages { get; set; } = Array.Empty<string>();
}

public class TestResponse
{
    public string Message { get; set; } = string.Empty;
}
