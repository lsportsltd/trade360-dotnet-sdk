using System;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Markets
{
    public class BaseBet
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Line { get; set; }

        public string? BaseLine { get; set; }

        public BetStatus Status { get; set; }

        public string? StartPrice { get; set; }

        public string? Price { get; set; }
        
        public SettlementType? Settlement { get; set; }
        
        public int? SuspensionReason { get; set; }

        public DateTime LastUpdate { get; set; }

        public double Probability { get; set; }

        public int? ParticipantId { get; set; }

        public string? PlayerName { get; set; }
    }
}