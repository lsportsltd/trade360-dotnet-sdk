using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetVenuesRequest : BaseMetadataRequest
    {
        public VenueFilter? Filter { get; set; }
    }
    
    public class VenueFilter
    {
        public IEnumerable<int>? VenueIds { get; set; }
        public IEnumerable<int>? CountryIds { get; set; }
        public IEnumerable<int>? StateIds { get; set; }
        public IEnumerable<int>? CityIds { get; set; }
    }
}