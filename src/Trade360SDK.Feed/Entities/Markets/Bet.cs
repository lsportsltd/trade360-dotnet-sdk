using System;
using Trade360SDK.Feed.Entities.Enums;

namespace Trade360SDK.Feed.Entities.Markets
{
    public class Bet
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Line { get; set; }

        public string? BaseLine { get; set; }

        public BetStatus Status { get; set; }

        public string? StartPrice { get; set; }

        public string? Price { get; set; }

        public string? LayPrice { get; set; }

        public string? PriceVolume { get; set; }

        public string? LayPriceVolume { get; set; }
        
        public SettlementType Settlement { get; set; }

        public string? ProviderBetId { get; set; }

        public DateTime LastUpdate { get; set; }

        public string? PriceIN { get; set; }

        public string? PriceUS { get; set; }

        public string? PriceUK { get; set; }

        public string? PriceMA { get; set; }

        public string? PriceHK { get; set; }

        public int IsChanged { get; set; } = -1;

        public double Probability { get; set; }

        public int ParticipantId { get; set; }

        public string? PlayerName { get; set; }
    }
}
