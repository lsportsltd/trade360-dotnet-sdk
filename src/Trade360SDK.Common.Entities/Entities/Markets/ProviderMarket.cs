using System;
using System.Collections.Generic;

namespace Trade360SDK.Common.Entities.Markets
{
    public class ProviderMarket
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public IEnumerable<ProviderBet>? Bets { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}