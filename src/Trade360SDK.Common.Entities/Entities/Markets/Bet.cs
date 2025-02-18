using System;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Markets
{
    public class Bet : BaseBet
    {
        public string? ProviderBetId { get; set; }
    }
}
