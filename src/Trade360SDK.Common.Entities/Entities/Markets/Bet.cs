using System;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Markets
{
    public class Bet : BaseBet
    {
        public string? PriceVolume { get; set; }
        
        public string? ProviderBetId { get; set; }

        public double? PriceIN { get; set; }

        public double? PriceUS { get; set; }

        public double? PriceUK { get; set; }

        public double? PriceMA { get; set; }

        public double? PriceHK { get; set; }

        public int IsChanged { get; set; } = -1;
    }
}
