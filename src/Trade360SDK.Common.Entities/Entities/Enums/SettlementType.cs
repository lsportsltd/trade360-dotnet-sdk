namespace Trade360SDK.Common.Entities.Enums
{
    public enum SettlementType
    {
        Cancelled = -1,
        NotSettled = 0,
        Loser = 1,
        Winner = 2,
        Refund = 3,
        HalfLost = 4,
        HalfWon = 5
    }
}
