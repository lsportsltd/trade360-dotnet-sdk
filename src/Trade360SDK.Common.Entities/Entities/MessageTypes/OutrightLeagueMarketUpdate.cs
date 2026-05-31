using System;
using System.Text.Json.Serialization;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.OutrightLeague;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(40)]
    public class OutrightLeagueMarketUpdate : MessageUpdate
    {
        [JsonConverter(typeof(OutrightLeagueMarketCompetitionWrapperConverter))]
        public OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>? Competition { get; set; }

        public DateTime? GetNextFixtureStartTime() =>
            (Competition as OutrightLeagueMarketCompetitionWrapper<OutrightLeagueMarketEvent>)?.NextFixtureStartTime;
    }
}
