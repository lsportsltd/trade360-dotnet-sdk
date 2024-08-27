using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.OutrightSport;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(39)]
    public class OutrightScoreUpdate : MessageUpdate
    {
        public OutrightCompetition<OutrightScoreEvent>? Competition { get; set; }
    }
}
