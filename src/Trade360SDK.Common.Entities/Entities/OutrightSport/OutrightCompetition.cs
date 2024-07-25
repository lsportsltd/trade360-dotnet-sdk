using System.Collections.Generic;

namespace Trade360SDK.Common.Entities.OutrightSport
{
    public class OutrightCompetition<TEvent>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Type { get; set; }
        public IEnumerable<TEvent>? Events { get; set; }
    }
}
