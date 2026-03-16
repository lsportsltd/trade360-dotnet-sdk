using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Responses;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.BetBuilderApi.Responses
{
    public class BetBuilderResponseTests
    {
        [Fact]
        public void BetBuilderResponse_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            var response = new BetBuilderResponse();

            response.Should().NotBeNull();
            response.Status.Should().BeNull();
            response.Metadata.Should().BeNull();
            response.Response.Should().BeNull();
        }

        [Fact]
        public void BetBuilderResponse_SetProperties_ShouldReturnCorrectValues()
        {
            var metadata = new BetBuilderResponseMetadata();
            var body = new BetBuilderResponseBody();

            var response = new BetBuilderResponse
            {
                Status = "OK",
                Metadata = metadata,
                Response = body
            };

            response.Status.Should().Be("OK");
            response.Metadata.Should().BeSameAs(metadata);
            response.Response.Should().BeSameAs(body);
        }
    }
}
