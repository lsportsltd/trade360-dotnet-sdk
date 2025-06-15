using FluentAssertions;
using Trade360SDK.CustomersApi.Entities.Base;

namespace Trade360SDK.CustomersApi.Tests;

public class HeaderResponsePropertyTests
{
    [Fact]
    public void RequestId_Property_ShouldSetAndGetValue()
    {
        var headerResponse = new HeaderResponse();
        var requestId = "test-request-123";

        headerResponse.RequestId = requestId;
        var retrievedId = headerResponse.RequestId;

        retrievedId.Should().Be(requestId);
    }

    [Fact]
    public void RequestId_WithNullValue_ShouldSetAndGetNull()
    {
        var headerResponse = new HeaderResponse
        {
            RequestId = null
        };

        headerResponse.RequestId.Should().BeNull();
    }

    [Fact]
    public void HttpStatusCode_Property_ShouldSetAndGetValue()
    {
        var headerResponse = new HeaderResponse();
        var statusCode = System.Net.HttpStatusCode.OK;

        headerResponse.HttpStatusCode = statusCode;
        var retrievedStatusCode = headerResponse.HttpStatusCode;

        retrievedStatusCode.Should().Be(statusCode);
    }

    [Fact]
    public void Errors_Property_ShouldSetAndGetValue()
    {
        var headerResponse = new HeaderResponse();
        var errors = new List<Error>
        {
            new Error { Message = "Test error" }
        };

        headerResponse.Errors = errors;
        var retrievedErrors = headerResponse.Errors;

        retrievedErrors.Should().BeEquivalentTo(errors);
    }
}
