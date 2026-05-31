using System;
using System.Collections.Generic;

namespace Trade360SDK.Common.Entities.OutrightLeague
{
    public class OutrightLeagueMarketCompetitionWrapper<TEvent> : OutrightLeagueCompetitionWrapper<TEvent>
    {
        public DateTime? NextFixtureStartTime { get; set; }
    }
}
