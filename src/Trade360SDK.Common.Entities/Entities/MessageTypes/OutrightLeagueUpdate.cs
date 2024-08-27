using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.OutrightLeague;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(38)]
    public class OutrightLeagueUpdate : MessageUpdate
    {
        public OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>? Competition { get; set; }
    }
}
