using System;
using System.Text.Json;
using FluentAssertions;
using Trade360SDK.Common.Entities.OutrightLeague;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class OutrightLeagueMarketCompetitionWrapperConverterTests
    {
        private static readonly JsonSerializerOptions FeedJsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly OutrightLeagueMarketCompetitionWrapperConverter _converter = new();

        [Fact]
        public void Write_WithMarketWrapper_ShouldSerializeNextFixtureStartTime()
        {
            var wrapper = new OutrightLeagueMarketCompetitionWrapper<OutrightLeagueMarketEvent>
            {
                Id = 67,
                Name = "League_67",
                Type = 3,
                NextFixtureStartTime = new DateTime(2026, 5, 29, 14, 44, 55, DateTimeKind.Utc),
            };

            using var document = SerializeToDocument(wrapper);

            document.RootElement.GetProperty("id").GetInt32().Should().Be(67);
            document.RootElement.GetProperty("nextFixtureStartTime").GetDateTime()
                .Should().Be(new DateTime(2026, 5, 29, 14, 44, 55, DateTimeKind.Utc));
        }

        [Fact]
        public void Write_WithNullValue_ShouldSerializeNull()
        {
            using var document = SerializeToDocument(null);

            document.RootElement.ValueKind.Should().Be(JsonValueKind.Null);
        }

        [Fact]
        public void ReadAndWrite_ShouldRoundTripMarketWrapper()
        {
            var options = new JsonSerializerOptions(FeedJsonOptions);
            options.Converters.Add(new OutrightLeagueMarketCompetitionWrapperConverter());

            var original = new OutrightLeagueMarketCompetitionWrapper<OutrightLeagueMarketEvent>
            {
                Id = 67,
                Name = "League_67",
                Type = 3,
                NextFixtureStartTime = new DateTime(2026, 5, 29, 14, 44, 55, DateTimeKind.Utc),
            };

            var json = JsonSerializer.Serialize(original, options);
            var roundTripped = JsonSerializer.Deserialize<OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>>(
                json,
                options);

            roundTripped.Should().BeOfType<OutrightLeagueMarketCompetitionWrapper<OutrightLeagueMarketEvent>>();
            var marketWrapper = (OutrightLeagueMarketCompetitionWrapper<OutrightLeagueMarketEvent>)roundTripped!;
            marketWrapper.NextFixtureStartTime.Should().Be(new DateTime(2026, 5, 29, 14, 44, 55, DateTimeKind.Utc));
        }

        private JsonDocument SerializeToDocument(OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>? value)
        {
            using var stream = new System.IO.MemoryStream();
            using (var writer = new Utf8JsonWriter(stream))
            {
                _converter.Write(writer, value, FeedJsonOptions);
            }

            stream.Position = 0;
            return JsonDocument.Parse(stream);
        }
    }
}
