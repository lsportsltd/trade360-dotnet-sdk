using System.Collections.Generic;

namespace Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Responses
{
    public class BetBuilderMargined
    {
        public string? Status { get; set; }

        public IEnumerable<double>? Pcb { get; set; }

        public double? RevenueTax { get; set; }

        public double Value { get; set; }
    }
}
