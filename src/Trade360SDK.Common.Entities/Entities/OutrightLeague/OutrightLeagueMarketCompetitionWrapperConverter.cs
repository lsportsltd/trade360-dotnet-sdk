using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Trade360SDK.Common.Entities.OutrightLeague
{
    internal sealed class OutrightLeagueMarketCompetitionWrapperConverter
        : JsonConverter<OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>?>
    {
        public override OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<OutrightLeagueMarketCompetitionWrapper<OutrightLeagueMarketEvent>>(
                ref reader,
                options);
        }

        public override void Write(
            Utf8JsonWriter writer,
            OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>? value,
            JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(
                writer,
                value,
                value?.GetType() ?? typeof(OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>),
                options);
        }
    }
}
