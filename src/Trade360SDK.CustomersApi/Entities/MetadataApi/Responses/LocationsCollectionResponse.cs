using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class LocationsCollectionResponse
    {
        public IEnumerable<Location>? Locations { get; set; }
    }

    public class Location
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
