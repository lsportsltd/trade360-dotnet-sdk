using System.Collections.Generic;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Requests
{
    public class ChangeManualSuspensionRequestDto
    {
        public List<Suspension> Suspensions { get; set; }
    }

}
