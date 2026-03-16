using System.Collections.Generic;

namespace Trade360SDK.BetBuilderApi.Entities.MappingsApi.Responses
{
    public class GetMappingsResponse
    {
        public IEnumerable<MappingEntry>? Mappings { get; set; }
    }

    public class MappingEntry
    {
        public int SportId { get; set; }

        public int LsportsMarketId { get; set; }

        public string? TradeMarketCode { get; set; }

        public string? TradeDataEndpoint { get; set; }

        public SelectionSchema? SelectionSchema { get; set; }

        public IEnumerable<string>? Periods { get; set; }

        public string? Status { get; set; }

        public string? Version { get; set; }
    }

    public class SelectionSchema
    {
        public string? Over { get; set; }

        public string? Under { get; set; }

        public string? LineParam { get; set; }

        public string? Home { get; set; }

        public string? Away { get; set; }

        public string? Draw { get; set; }
    }
}
