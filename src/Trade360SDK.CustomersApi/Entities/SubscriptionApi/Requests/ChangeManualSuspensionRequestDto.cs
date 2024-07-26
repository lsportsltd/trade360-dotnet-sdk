using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests
{
    public class ChangeManualSuspensionRequestDto
    {
        public List<Suspension>? Suspensions { get; set; }
    }
}