using System.Collections.Generic;

namespace Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Requests
{
    public class BetBuilderMarketPeriod
    {
        public Dictionary<string, Dictionary<string, double>>? AsianTotals { get; set; }

        public Dictionary<string, Dictionary<string, double>>? EuropeanHandicaps { get; set; }

        public Dictionary<string, Dictionary<string, double>>? AsianHandicaps { get; set; }

        public Dictionary<string, Dictionary<string, double>>? EuropeanFirstTos { get; set; }

        public Dictionary<string, Dictionary<string, double>>? AsianFirstTos { get; set; }

        public Dictionary<string, double>? CorrectScore { get; set; }

        public Dictionary<string, Dictionary<string, double>>? NthPlayers { get; set; }

        public Dictionary<string, Dictionary<string, Dictionary<string, double>>>? AsianPlayerTotals { get; set; }

        public Dictionary<string, Dictionary<string, Dictionary<string, double>>>? AsianPlayerPasses { get; set; }

        public Dictionary<string, Dictionary<string, Dictionary<string, double>>>? AsianPlayerRushes { get; set; }

        public Dictionary<string, Dictionary<string, Dictionary<string, double>>>? AsianPlayerReceives { get; set; }

        public Dictionary<string, Dictionary<string, Dictionary<string, double>>>? AsianPlayerRushReceives { get; set; }
    }
}
