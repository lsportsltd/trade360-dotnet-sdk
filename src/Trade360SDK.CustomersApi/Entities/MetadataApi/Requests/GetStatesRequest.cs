using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetStatesRequest : BaseMetadataRequest
    {
        public StateFilter? Filter { get; set; }
    }
    
    public class StateFilter
    {
        public IEnumerable<int>? CountryIds { get; set; }
        public IEnumerable<int>? StateIds { get; set; }
    }
}