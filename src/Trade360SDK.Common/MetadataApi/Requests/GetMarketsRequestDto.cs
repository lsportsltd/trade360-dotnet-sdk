using System.Collections.Generic;
using Trade360SDK.Api.Abstraction.Enums;

namespace Trade360SDK.Api.Abstraction.MetadataApi.Requests
{
    public class GetMarketsRequestDto
    {
        public IEnumerable<int>? SportIds { get; set; }
        public IEnumerable<int>? LocationIds { get; set; }
        public IEnumerable<int>? LeagueIds { get; set; }
        public IEnumerable<int>? MarketIds { get; set; }
        public bool? IsSettleable { get; set; }
        public MarketTypeEnum MarketType { get; set; }
        public int? LanguageId { get; set; }
    }
}
