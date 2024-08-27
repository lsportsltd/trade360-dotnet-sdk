using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.OutrightSport;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(41)]
    public class OutrightFixtureMarketMessageUpdate : MessageUpdate
    {
        public OutrightCompetition<MarketEvent>? Competition { get; set; }
    }
}
