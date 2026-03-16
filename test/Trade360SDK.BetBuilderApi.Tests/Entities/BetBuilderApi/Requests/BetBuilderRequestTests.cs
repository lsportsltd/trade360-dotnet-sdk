using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Requests;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.BetBuilderApi.Requests
{
    public class BetBuilderRequestTests
    {
        [Fact]
        public void BetBuilderRequest_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            var request = new BetBuilderRequest();

            request.Should().NotBeNull();
            request.CustomerId.Should().Be(0);
            request.UserId.Should().BeNull();
            request.EvId.Should().Be(0);
            request.MsgType.Should().BeNull();
            request.Request.Should().BeNull();
            request.Metadata.Should().BeNull();
        }

        [Fact]
        public void BetBuilderRequest_SetProperties_ShouldReturnCorrectValues()
        {
            var body = new BetBuilderRequestBody();
            var metadata = new BetBuilderRequestMetadata { CorrelationId = "test-id" };

            var request = new BetBuilderRequest
            {
                CustomerId = 123,
                UserId = "user-1",
                EvId = 456,
                MsgType = "derive",
                Request = body,
                Metadata = metadata
            };

            request.CustomerId.Should().Be(123);
            request.UserId.Should().Be("user-1");
            request.EvId.Should().Be(456);
            request.MsgType.Should().Be("derive");
            request.Request.Should().BeSameAs(body);
            request.Metadata.Should().BeSameAs(metadata);
        }
    }
}
