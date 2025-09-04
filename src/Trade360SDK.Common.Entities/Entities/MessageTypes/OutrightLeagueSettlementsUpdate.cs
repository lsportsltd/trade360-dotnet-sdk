using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.OutrightLeague;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(43)]
    public class OutrightLeagueSettlementUpdate :  MessageUpdate
    {
        public OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>? Competition { get; set; }
    }
}