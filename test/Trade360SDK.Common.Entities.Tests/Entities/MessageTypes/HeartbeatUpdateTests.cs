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
        public void FeedInterrupted_ShouldDefaultToEmptyArray()
        {
            var update = new HeartbeatUpdate();

            update.FeedInterrupted.Should().BeEmpty();
        }

        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var update = new HeartbeatUpdate
            {
                FeedInterrupted = [(int)FeedInterruptedDomainEnum.Markets]
            };

            update.FeedInterrupted.Should().Equal(1);
        }

        [Fact]
        public void HeartbeatUpdate_ShouldHaveTrade360EntityAttributeWithKey32()
        {
            var attributes = typeof(HeartbeatUpdate).GetCustomAttributes(typeof(Trade360EntityAttribute), false);

            attributes.Should().HaveCount(1);
            ((Trade360EntityAttribute)attributes[0]).EntityKey.Should().Be(32);
        }

        [Fact]
        public void JsonDeserialization_WithFeedInterruptedArrayInBody_ShouldPopulate()
        {
            const string body = "{\"FeedInterrupted\":[1]}";

            var result = JsonSerializer.Deserialize<HeartbeatUpdate>(body, FeedJsonOptions);

            result.Should().NotBeNull();
            result!.FeedInterrupted.Should().Equal(1);
        }

        [Fact]
        public void JsonDeserialization_WithoutFeedInterruptedInBody_ShouldDefaultToEmpty()
        {
            const string body = "{}";

            var result = JsonSerializer.Deserialize<HeartbeatUpdate>(body, FeedJsonOptions);

            result.Should().NotBeNull();
            result!.FeedInterrupted.Should().BeEmpty();
        }
    }
}
