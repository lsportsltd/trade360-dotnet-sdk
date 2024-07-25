using System.Collections.Generic;

namespace Trade360SDK.Common.Entities.OutrightLeague
{
    public class OutrightLeagueEventsWrapper<TEvent>
    {
        public IEnumerable<TEvent>? Events { get; set; }
    }
}
