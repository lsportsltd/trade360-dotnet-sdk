using FluentAssertions;
using Trade360SDK.CustomersApi.Entities.Base;

namespace Trade360SDK.CustomersApi.Tests;

public class HeaderResponseCoverageTests
{
    [Fact]
    public void RequestId_Property_ShouldExecutePropertyAccess()
    {
        var headerResponse = new HeaderResponse();
        var requestId = "test-request-123";

        headerResponse.RequestId = requestId;
        var retrievedId = headerResponse.RequestId;

        retrievedId.Should().Be(requestId);
    }

    [Fact]
    public void RequestId_WithNullValue_ShouldAllowNull()
    {
        var headerResponse = new HeaderResponse
        {
            RequestId = null
        };

        headerResponse.RequestId.Should().BeNull();
    }

    [Fact]
    public void RequestId_WithEmptyString_ShouldAllowEmpty()
    {
        var headerResponse = new HeaderResponse
        {
            RequestId = string.Empty
        };

        headerResponse.RequestId.Should().Be(string.Empty);
    }
}
