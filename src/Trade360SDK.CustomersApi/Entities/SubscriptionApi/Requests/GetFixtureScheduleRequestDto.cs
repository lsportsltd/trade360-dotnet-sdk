using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests
{
    public class GetFixtureScheduleRequestDto
    {
        public IEnumerable<int>? SportIds { get; set; }
        public IEnumerable<int>? LocationIds { get; set; }
        public IEnumerable<int>? LeagueIds { get; set; }

    }
}
