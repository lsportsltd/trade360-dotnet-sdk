using System.Collections.Generic;
using Trade360SDK.Feed.Attributes;
using Trade360SDK.Feed.Entities.Fixtures;

namespace Trade360SDK.Feed.Entities
{
    [Trade360Entity(1)]
    public class FixtureMetadataUpdate
    {
        public IEnumerable<FixtureEvent>? Events { get; set; }
    }
}
