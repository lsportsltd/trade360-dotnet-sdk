using FluentAssertions;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Http;
using System.Reflection;

namespace Trade360SDK.CustomersApi.Tests;

public class BaseHttpClientQueryStringTests
{
    private class TestHttpClient : BaseHttpClient
    {
        public TestHttpClient(IHttpClientFactory factory, string baseUrl, PackageCredentials credentials) 
            : base(factory, baseUrl, credentials) { }

        public string TestBuildQueryString(object request)
        {
            var method = typeof(BaseHttpClient).GetMethod("BuildQueryString", BindingFlags.NonPublic | BindingFlags.Instance);
            return (string)method!.Invoke(this, new[] { request });
        }
    }

    [Fact]
    public void BuildQueryString_WithNullRequest_ShouldReturnEmptyString()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var client = new TestHttpClient(mockFactory.Object, "https://api.test.com", credentials);

        var result = client.TestBuildQueryString(null!);

        result.Should().Be(string.Empty);
    }

    [Fact]
    public void BuildQueryString_WithSimpleObject_ShouldCreateQueryString()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var client = new TestHttpClient(mockFactory.Object, "https://api.test.com", credentials);

        var request = new { SportId = 1, Name = "Football" };
        var result = client.TestBuildQueryString(request);

        result.Should().Contain("SportId=1");
        result.Should().Contain("Name=Football");
    }

    [Fact]
    public void BuildQueryString_WithArrayProperty_ShouldCreateMultipleQueryParams()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var client = new TestHttpClient(mockFactory.Object, "https://api.test.com", credentials);

        var request = new { SportIds = new[] { 1, 2, 3 } };
        var result = client.TestBuildQueryString(request);

        result.Should().Contain("SportIds=1");
        result.Should().Contain("SportIds=2");
        result.Should().Contain("SportIds=3");
    }

    [Fact]
    public void BuildQueryString_WithNullPropertyValue_ShouldSkipProperty()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var client = new TestHttpClient(mockFactory.Object, "https://api.test.com", credentials);

        var request = new { SportId = 1, Name = (string)null! };
        var result = client.TestBuildQueryString(request);

        result.Should().Contain("SportId=1");
        result.Should().NotContain("Name=");
    }

    [Fact]
    public void BuildQueryString_WithEmptyArray_ShouldSkipProperty()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var client = new TestHttpClient(mockFactory.Object, "https://api.test.com", credentials);

        var request = new { SportIds = new int[0] };
        var result = client.TestBuildQueryString(request);

        result.Should().NotContain("SportIds=");
    }
}
