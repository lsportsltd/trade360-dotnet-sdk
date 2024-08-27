using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.OutrightLeague;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(40)]
    public class OutrightLeagueMarketUpdate : MessageUpdate
    {
        public OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>? Competition { get; set; }
    }
}
