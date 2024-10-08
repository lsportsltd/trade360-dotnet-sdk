﻿using System.Collections.Generic;
using Trade360SDK.CustomersApi.Entities.Base;

namespace Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests
{
    public class GetFixtureScheduleRequest : BaseRequest
    {
        public IEnumerable<int>? SportIds { get; set; }
        public IEnumerable<int>? LocationIds { get; set; }
        public IEnumerable<int>? LeagueIds { get; set; }

    }
}
