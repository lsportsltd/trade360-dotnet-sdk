using System.Collections.Generic;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class GetStatesResponse
    {
        public IEnumerable<State>? Data { get; set; }
    }

    public class State
    {
        public int StateId { get; set; }
        public string Name { get; set; }
        public IdNamePair? Country { get; set; }
    }
}