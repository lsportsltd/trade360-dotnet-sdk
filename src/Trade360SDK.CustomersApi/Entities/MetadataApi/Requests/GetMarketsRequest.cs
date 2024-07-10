﻿using System.Collections.Generic;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.CustomersApi.Entities.Base;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
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
