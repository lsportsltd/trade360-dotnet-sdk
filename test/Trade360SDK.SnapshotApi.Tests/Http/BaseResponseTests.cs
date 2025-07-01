using FluentAssertions;
using Trade360SDK.Common.Models;
using Trade360SDK.SnapshotApi.Http;

namespace Trade360SDK.SnapshotApi.Tests.Http;

public class BaseResponseTests
{
    [Fact]
    public void BaseResponse_DefaultConstructor_ShouldInitializeWithNullProperties()
    {
        // Act
        var response = new BaseResponse<string>();

        // Assert
        response.Should().NotBeNull();
        response.Header.Should().BeNull();
        response.Body.Should().BeNull();
    }

    [Fact]
    public void BaseResponse_HeaderProperty_ShouldBeSettableAndGettable()
    {
        // Arrange
        var response = new BaseResponse<string>();
        var header = new MessageHeader
        {
            CreationDate = "2024-01-01T00:00:00.000Z",
            Type = 1,
            MsgSeq = 123,
            MsgGuid = Guid.NewGuid().ToString(),
            ServerTimestamp = 1640995200000,
            MessageBrokerTimestamp = DateTime.UtcNow
        };

        // Act
        response.Header = header;

        // Assert
        response.Header.Should().Be(header);
        response.Header.Type.Should().Be(1);
        response.Header.MsgSeq.Should().Be(123);
    }

    [Fact]
    public void BaseResponse_BodyProperty_ShouldBeSettableAndGettable()
    {
        // Arrange
        var response = new BaseResponse<string>();
        var body = "Test body content";

        // Act
        response.Body = body;

        // Assert
        response.Body.Should().Be(body);
    }

    [Fact]
    public void BaseResponse_WithComplexObject_ShouldMaintainReferenceIntegrity()
    {
        // Arrange
        var response = new BaseResponse<List<int>>();
        var body = new List<int> { 1, 2, 3 };

        // Act
        response.Body = body;
        body.Add(4);

        // Assert
        response.Body.Should().Contain(4);
        response.Body.Should().HaveCount(4);
    }

    [Fact]
    public void BaseResponse_WithNullBody_ShouldAcceptNull()
    {
        // Arrange
        var response = new BaseResponse<string>();

        // Act
        response.Body = null;

        // Assert
        response.Body.Should().BeNull();
    }

    [Fact]
    public void BaseResponse_WithNullHeader_ShouldAcceptNull()
    {
        // Arrange
        var response = new BaseResponse<string>();

        // Act
        response.Header = null;

        // Assert
        response.Header.Should().BeNull();
    }

    [Fact]
    public void BaseResponse_GenericType_ShouldWorkWithDifferentTypes()
    {
        // Arrange & Act
        var stringResponse = new BaseResponse<string> { Body = "test" };
        var objectResponse = new BaseResponse<object> { Body = 42 }; // Using object to wrap the int
        var listResponse = new BaseResponse<List<string>> { Body = new List<string> { "item1" } };

        // Assert
        stringResponse.Body.Should().Be("test");
        objectResponse.Body.Should().Be(42);
        listResponse.Body.Should().HaveCount(1).And.Contain("item1");
    }

    [Fact]
    public void BaseResponse_PropertyAssignment_ShouldSupportChaining()
    {
        // Arrange
        var header = new MessageHeader { Type = 1 };
        var body = "test data";

        // Act
        var response = new BaseResponse<string>
        {
            Header = header,
            Body = body
        };

        // Assert
        response.Header.Should().Be(header);
        response.Body.Should().Be(body);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("normal string")]
    [InlineData("string with\nnewlines")]
    public void BaseResponse_WithVariousStringBodies_ShouldHandleCorrectly(string bodyContent)
    {
        // Arrange & Act
        var response = new BaseResponse<string> { Body = bodyContent };

        // Assert
        response.Body.Should().Be(bodyContent);
    }

    [Fact]
    public void BaseResponse_TypeConstraint_ShouldOnlyAcceptReferenceTypes()
    {
        // This test verifies the generic constraint 'where TBody : class'
        // The following should compile fine:
        var stringResponse = new BaseResponse<string>();
        var objectResponse = new BaseResponse<object>();
        var listResponse = new BaseResponse<List<int>>();

        // Assert compilation success
        stringResponse.Should().NotBeNull();
        objectResponse.Should().NotBeNull();
        listResponse.Should().NotBeNull();

        // Note: BaseResponse<int> would not compile due to the class constraint
        // This is enforced at compile time, not runtime
    }
} 