using System;

namespace Trade360SDK.Common.Entities.Markets
{
    public class PredictionData
    {
        public double? Volume { get; set; }

        public double? Liquidity { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
