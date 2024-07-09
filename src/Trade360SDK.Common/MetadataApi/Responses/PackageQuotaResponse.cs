﻿using System;

namespace Trade360SDK.Api.Abstraction.MetadataApi.Responses
{
    public class PackageQuotaResponse
    {
        public int CreditRemaining { get; set; }
        public int CreditLimit { get; set; }
        public int UsedCredit { get; set; }
        public DateTime CurrentPeriodStartDate { get; set; }
        public DateTime CurrentPeriodEndDate { get; set; }
    }
}
