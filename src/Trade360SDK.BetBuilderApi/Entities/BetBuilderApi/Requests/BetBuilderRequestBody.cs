namespace Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Requests
{
    public class BetBuilderRequestBody
    {
        public BetBuilderContest? Contest { get; set; }

        public BetBuilderModel? Model { get; set; }

        public BetBuilderBet? Bet { get; set; }
    }
}
