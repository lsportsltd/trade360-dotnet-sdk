using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.OutrightSport;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(37)]
    public class OutrightFixtureMessageUpdate : MessageUpdate
    {
        public OutrightCompetition<OutrightFixtureEvent>? Competition { get; set; }
    }
}
