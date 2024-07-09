using System.Collections.Generic;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Requests
{
    public class GetFixtureScheduleRequest : BaseRequest
    {
        public IEnumerable<int>? SportIds { get; set; }
        public IEnumerable<int>? LocationIds { get; set; }
        public IEnumerable<int>? LeagueIds { get; set; }

    }
}
