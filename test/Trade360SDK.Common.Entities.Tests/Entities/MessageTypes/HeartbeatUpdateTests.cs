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
        public void FeedInterrupted_ShouldDefaultToZero()
        {
            var update = new HeartbeatUpdate();

            update.FeedInterrupted.Should().Be(0);
        }

        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var update = new HeartbeatUpdate
            {
                FeedInterrupted = 1
            };

            update.FeedInterrupted.Should().Be(1);
        }

        [Fact]
        public void HeartbeatUpdate_ShouldHaveTrade360EntityAttributeWithKey32()
        {
            var attributes = typeof(HeartbeatUpdate).GetCustomAttributes(typeof(Trade360EntityAttribute), false);

            attributes.Should().HaveCount(1);
            ((Trade360EntityAttribute)attributes[0]).EntityKey.Should().Be(32);
        }

        [Fact]
        public void JsonDeserialization_WithFeedInterruptedInBody_ShouldPopulate()
        {
            const string body = "{\"FeedInterrupted\":1}";

            var result = JsonSerializer.Deserialize<HeartbeatUpdate>(body, FeedJsonOptions);

            result.Should().NotBeNull();
            result!.FeedInterrupted.Should().Be(1);
        }

        [Fact]
        public void JsonDeserialization_WithoutFeedInterruptedInBody_ShouldDefaultToZero()
        {
            const string body = "{}";

            var result = JsonSerializer.Deserialize<HeartbeatUpdate>(body, FeedJsonOptions);

            result.Should().NotBeNull();
            result!.FeedInterrupted.Should().Be(0);
        }

        [Fact]
        public void JsonDeserialization_WithCamelCaseFeedInterrupted_ShouldPopulate()
        {
            const string body = "{\"feedInterrupted\":1}";

            var result = JsonSerializer.Deserialize<HeartbeatUpdate>(body, FeedJsonOptions);

            result.Should().NotBeNull();
            result!.FeedInterrupted.Should().Be(1);
        }
    }
}
