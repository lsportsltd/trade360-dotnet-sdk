using System.Collections.Generic;
using Trade360SDK.Common.Entities.Fixtures;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class GetStatesResponse
    {
        public IEnumerable<State>? Data { get; set; }
    }
}