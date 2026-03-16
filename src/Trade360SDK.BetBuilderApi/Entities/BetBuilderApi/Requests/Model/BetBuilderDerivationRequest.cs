using System.Collections.Generic;

namespace Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Requests
{
    public class BetBuilderDerivationRequest
    {
        public Dictionary<string, BetBuilderMarketCategory>? Markets { get; set; }
    }
}
