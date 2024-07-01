﻿using System.Collections.Generic;

namespace Trade360SDK.Common.Metadata.Responses
{
    public class LocationsCollection
    {
        public IEnumerable<Location>? Locations { get; set; }
    }

    public class Location
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
