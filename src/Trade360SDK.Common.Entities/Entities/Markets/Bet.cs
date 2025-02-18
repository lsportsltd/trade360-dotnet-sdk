using System;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Markets
{
    public class Bet : BaseBet
    {
        public string? PriceVolume { get; set; }
        
        public string? ProviderBetId { get; set; }

        public string? PriceIN { get; set; }

        public string? PriceUS { get; set; }

        public string? PriceUK { get; set; }

        public string? PriceMA { get; set; }

        public string? PriceHK { get; set; }

        public int? IsChanged { get; set; } = -1;
    }
}
