using System.Text.Json;
using FluentAssertions;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.MessageTypes;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.MessageTypes
{
    public class HeartbeatUpdateTests
    {
        private static readonly JsonSerializerOptions FeedJsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        [Fact]
        public void Problem_ShouldDefaultToZero()
        {
            var update = new HeartbeatUpdate();

            update.Problem.Should().Be(0);
        }

        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var update = new HeartbeatUpdate
            {
                Problem = 1
            };

            update.Problem.Should().Be(1);
        }

        [Fact]
        public void HeartbeatUpdate_ShouldHaveTrade360EntityAttributeWithKey32()
        {
            var attributes = typeof(HeartbeatUpdate).GetCustomAttributes(typeof(Trade360EntityAttribute), false);

            attributes.Should().HaveCount(1);
            ((Trade360EntityAttribute)attributes[0]).EntityKey.Should().Be(32);
        }

        [Fact]
        public void JsonDeserialization_WithProblemInBody_ShouldPopulateProblem()
        {
            const string body = "{\"Problem\":1}";

            var result = JsonSerializer.Deserialize<HeartbeatUpdate>(body, FeedJsonOptions);

            result.Should().NotBeNull();
            result!.Problem.Should().Be(1);
        }

        [Fact]
        public void JsonDeserialization_WithoutProblemInBody_ShouldDefaultToZero()
        {
            const string body = "{}";

            var result = JsonSerializer.Deserialize<HeartbeatUpdate>(body, FeedJsonOptions);

            result.Should().NotBeNull();
            result!.Problem.Should().Be(0);
        }

        [Fact]
        public void JsonDeserialization_WithLowercaseProblemInBody_ShouldPopulateProblem()
        {
            const string body = "{\"problem\":1}";

            var result = JsonSerializer.Deserialize<HeartbeatUpdate>(body, FeedJsonOptions);

            result.Should().NotBeNull();
            result!.Problem.Should().Be(1);
        }
    }
}
