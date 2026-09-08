namespace Trade360SDK.Common.Entities.Enums
{
    public enum RefundReason
    {
        NotSupported = -1,
        NotSet = 0,
        MarketSettlementRules = 1,
        EventCancelled = 2,
        EventAbandoned = 3,
        PlayerNotPlayed = 4,
        PlayerNotInLineUp = 5,
        MinPeriodsNotReached = 6,
        MinBallsNotReached = 7,
        NonessentialMarket = 8,
    }
}
