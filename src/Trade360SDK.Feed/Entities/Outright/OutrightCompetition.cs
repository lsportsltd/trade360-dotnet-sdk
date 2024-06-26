using System;
using System.Collections.Generic;
using System.Text;

namespace Trade360SDK.Feed.Entities.Outright
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
