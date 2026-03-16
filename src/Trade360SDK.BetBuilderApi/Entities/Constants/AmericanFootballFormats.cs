namespace Trade360SDK.BetBuilderApi.Entities.Constants
{
    public static class AmericanFootballFormats
    {
        /// <summary>
        /// NFL Regular Season format: 4 quarters of 15 minutes + unlimited 15-minute overtime periods
        /// </summary>
        public const string NFLRegularSeason = "american-football/RT:15{4}+OT:15*";

        /// <summary>
        /// NFL Playoffs format: Same as regular season
        /// </summary>
        public const string NFLPlayoffs = "american-football/RT:15{4}+OT:15*";

        /// <summary>
        /// NCAA College Football format
        /// </summary>
        public const string NCAA = "american-football/ncaa/playoffs";
    }
}
