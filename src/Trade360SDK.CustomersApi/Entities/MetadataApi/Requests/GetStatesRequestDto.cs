using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetStatesRequestDto
    {
        public StateFilterDto? Filter { get; set; }
    }
    
    public class StateFilterDto
    {
        public IEnumerable<int>? CountryIds { get; set; }
        public IEnumerable<int>? StateIds { get; set; }
    }
}