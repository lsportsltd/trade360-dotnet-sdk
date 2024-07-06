﻿using System.Collections.Generic;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Responses
{
    public class CompetitionSubscribtionCollectionResponse
    {
        public List<CompetitionSubscriptionResponse> Subscription { get; set; }
    }

    public class CompetitionSubscriptionResponse
    {
        public int CompetitionId { get; set; }
        public int SportId { get; set; }
        public int LocationId { get; set; }
        public bool Success { get; set; }
    }
}
