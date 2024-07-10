using System.Collections.Generic;

namespace Trade360SDK.Common.Entities.Markets
{
    public class Market
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public IEnumerable<Bet>? Bets { get; set; }

        public string? MainLine { get; set; }
    }
}
