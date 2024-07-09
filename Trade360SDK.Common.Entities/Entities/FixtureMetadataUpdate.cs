﻿using System.Collections.Generic;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Fixtures;

namespace Trade360SDK.Common.Entities
{
    [Trade360Entity(1)]
    public class FixtureMetadataUpdate
    {
        public IEnumerable<FixtureEvent>? Events { get; set; }
    }
}