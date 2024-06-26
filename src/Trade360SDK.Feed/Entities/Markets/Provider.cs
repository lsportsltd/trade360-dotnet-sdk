using System;
using System.Collections.Generic;

namespace Trade360SDK.Feed.Entities.Markets
{
    public class Provider
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public DateTime LastUpdate { get; set; }

        public string? ProviderFixtureId { get; set; }

        public string? ProviderLeagueId { get; set; }

        public string? ProviderMarketId { get; set; }

        public IEnumerable<Bet>? Bets { get; set; }
    }
}
