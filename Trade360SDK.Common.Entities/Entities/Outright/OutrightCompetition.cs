using System.Collections.Generic;

namespace Trade360SDK.Common.Entities.Outright
{
    public class OutrightCompetition<TEvent>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Type { get; set; }
        public IEnumerable<OutrightCompetition<TEvent>>? Competitions { get; set; }
        public IEnumerable<TEvent>? Events { get; set; }
    }
}
