using System.Collections.Generic;

namespace Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Requests
{
    public class BetBuilderBet
    {
        public BetBuilderMarginScheme? MarginScheme { get; set; }

        public IEnumerable<BetBuilderSelection>? Selections { get; set; }
    }
}
