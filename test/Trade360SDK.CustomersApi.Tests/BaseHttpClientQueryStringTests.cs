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
    public void BuildQueryString_WithJsonElementArray_ShouldCreateMultipleParams()
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
    public void BuildQueryString_WithJsonElementNonNull_ShouldIncludeValue()
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
    public void BuildQueryString_WithArrayContainingNullAndEmptyValues_ShouldSkipInvalidValues()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var client = new TestHttpClient(mockFactory.Object, "https://api.test.com", credentials);

        var request = new { Values = new[] { "valid", "", null, "another" } };
        var result = client.TestBuildQueryString(request);

        result.Should().Contain("Values=valid");
        result.Should().Contain("Values=another");
        result.Should().NotContain("Values=&");
        result.Should().NotContain("Values==");
    }

    [Fact]
    public void BuildQueryString_WithNonJsonElementValue_ShouldHandleDirectValue()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var client = new TestHttpClient(mockFactory.Object, "https://api.test.com", credentials);

        var request = new { SportId = 1, Name = "Football", IsActive = true };
        var result = client.TestBuildQueryString(request);

        result.Should().Contain("SportId=1");
        result.Should().Contain("Name=Football");
        result.Should().Contain("IsActive=True");
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

    [Fact]
    public void BuildQueryString_WithMixedArrayValues_ShouldHandleNullsAndEmptyValues()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var client = new TestHttpClient(mockFactory.Object, "https://api.test.com", credentials);

        var request = new { Values = new[] { "valid", "", null, "another" } };
        var result = client.TestBuildQueryString(request);

        result.Should().Contain("Values=valid");
        result.Should().Contain("Values=another");
        result.Should().NotContain("Values=&");
    }

    [Fact]
    public void BuildQueryString_WithComplexObject_ShouldHandleNestedProperties()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var client = new TestHttpClient(mockFactory.Object, "https://api.test.com", credentials);

        var request = new { 
            SportId = 1, 
            Name = "Football",
            IsActive = true,
            Categories = new[] { "Premier", "Championship" }
        };
        var result = client.TestBuildQueryString(request);

        result.Should().Contain("SportId=1");
        result.Should().Contain("Name=Football");
        result.Should().Contain("IsActive=True");
        result.Should().Contain("Categories=Premier");
        result.Should().Contain("Categories=Championship");
    }

    [Fact]
    public void BuildQueryString_WithDateTimeProperty_ShouldFormatCorrectly()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var client = new TestHttpClient(mockFactory.Object, "https://api.test.com", credentials);

        var testDate = new DateTime(2023, 1, 1, 12, 0, 0);
        var request = new { StartDate = testDate };
        var result = client.TestBuildQueryString(request);

        result.Should().Contain("StartDate=");
        result.Should().Contain("2023");
    }
}
