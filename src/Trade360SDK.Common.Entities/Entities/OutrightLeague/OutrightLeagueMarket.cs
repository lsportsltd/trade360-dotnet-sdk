using System.Collections.Generic;
using Trade360SDK.Common.Entities.Markets;

namespace Trade360SDK.Common.Entities.OutrightLeague
{
    public class OutrightLeagueMarket
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<Bet>? Bets { get; set; }
    }
}