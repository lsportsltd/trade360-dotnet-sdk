using System.Collections.Generic;

namespace Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Responses
{
    public class BetBuilderDataBet
    {
        public BetBuilderMargined? Margined { get; set; }

        public BetBuilderExpectation? Expectation { get; set; }

        public IEnumerable<int>? RequiredSelections { get; set; }

        public double ExpectedRtp { get; set; }

        public IEnumerable<BetBuilderDataSelection>? Selections { get; set; }

        public string? BetType { get; set; }
    }
}
