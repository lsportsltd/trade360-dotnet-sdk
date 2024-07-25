using System.Collections.Generic;

namespace Trade360SDK.Common.Entities.OutrightLeague
{
    public class OutrightLeagueCompetitionWrapper<TEvent>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Type { get; set; }
        public IEnumerable<OutrightLeagueEventsWrapper<TEvent>>? Competitions { get; set; }
    }
}
