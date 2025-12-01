using System.Collections.Generic;
using Trade360SDK.Common.Entities.OutrightLeague;

namespace Trade360SDK.SnapshotApi.Entities.Responses
{
    public class GetOutrightLeagueEventsResponse
    {
        public IEnumerable<OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>> Competition { get; set; }
    }
}