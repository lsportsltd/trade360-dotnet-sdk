using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Trade360SDK.Common.Entities.OutrightLeague
{
    public class OutrightLeagueMarketEvent
    {
        public int FixtureId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FixtureName
        {
            get => _fixtureName;
            set => _fixtureName = string.IsNullOrEmpty(value) ? null : value;
        }

        public IEnumerable<MarketLeague>? Markets { get; set; }

        private string? _fixtureName;
    }
}
