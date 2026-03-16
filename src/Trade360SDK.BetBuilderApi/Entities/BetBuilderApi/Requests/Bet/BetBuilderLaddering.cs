using System.Collections.Generic;

namespace Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Requests
{
    public class BetBuilderLaddering
    {
        public string? Snap { get; set; }

        public IEnumerable<double>? Steps { get; set; }
    }
}
