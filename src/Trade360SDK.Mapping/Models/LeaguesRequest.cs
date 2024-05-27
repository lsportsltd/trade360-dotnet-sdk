using System.Collections.Generic;
using Trade360SDK.Common.Models;
using Trade360SDK.Mapping.Enums;

namespace Trade360SDK.Mapping.Models
{
    internal class LeaguesRequest : Request
    {
        public IEnumerable<int> SportIds { get; }
        public IEnumerable<int> LocationIds { get; }
        public SubscriptionStatus SubscriptionStatus { get; }

        public LeaguesRequest(
            IEnumerable<int> sportIds,
            IEnumerable<int> locationIds,
            SubscriptionStatus subscriptionStatus)
        {
            SportIds = sportIds;
            LocationIds = locationIds;
            SubscriptionStatus = subscriptionStatus;
        }
    }
}
