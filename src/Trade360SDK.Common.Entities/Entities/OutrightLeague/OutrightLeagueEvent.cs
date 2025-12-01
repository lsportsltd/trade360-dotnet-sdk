using System.Collections.Generic;

namespace Trade360SDK.Common.Entities.OutrightLeague
{
    public class OutrightLeagueEvent
    {
        public int FixtureId { get; set; }
        public OutrightLeagueFixture? OutrightLeague { get; set; }
        
        public IEnumerable<OutrightLeagueMarket>? Markets { get; set; }
    }
}
