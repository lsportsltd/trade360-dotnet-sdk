﻿using System.Collections.Generic;
using Trade360SDK.Api.Common.Models.Requests.Base;
using Trade360SDK.Metadata.Enums;

namespace Trade360SDK.Common.Metadata.Requests
{
    public class GetMarketsRequest : BaseRequest
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
