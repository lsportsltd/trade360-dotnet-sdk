using Trade360SDK.SnapshotApi.Entities.Requests;

namespace Trade360SDK.SnapshotApi.Mapper
{
    public static class SnapshotApiMapper
    {
        public static BaseStandardRequest Map(GetFixturesRequestDto source) => new BaseStandardRequest
        {
            Timestamp = source.Timestamp,
            FromDate = source.FromDate,
            ToDate = source.ToDate,
            Sports = source.Sports,
            Locations = source.Locations,
            Fixtures = source.Fixtures,
            Leagues = source.Leagues
        };

        public static BaseStandardRequest Map(GetLivescoreRequestDto source) => new BaseStandardRequest
        {
            Timestamp = source.Timestamp,
            FromDate = source.FromDate,
            ToDate = source.ToDate,
            Sports = source.Sports,
            Locations = source.Locations,
            Fixtures = source.Fixtures,
            Leagues = source.Leagues
        };

        public static BaseStandardRequest Map(GetMarketRequestDto source) => new BaseStandardRequest
        {
            Timestamp = source.Timestamp,
            FromDate = source.FromDate,
            ToDate = source.ToDate,
            Sports = source.Sports,
            Locations = source.Locations,
            Fixtures = source.Fixtures,
            Leagues = source.Leagues,
            Markets = source.Markets
        };

        public static BaseOutrightRequest Map(GetOutrightFixturesRequestDto source) => new BaseOutrightRequest
        {
            Timestamp = source.Timestamp,
            FromDate = source.FromDate,
            ToDate = source.ToDate,
            Sports = source.Sports,
            Locations = source.Locations,
            Fixtures = source.Fixtures,
            Tournaments = source.Tournaments
        };

        public static BaseOutrightRequest Map(GetOutrightLivescoreRequestDto source) => new BaseOutrightRequest
        {
            Timestamp = source.Timestamp,
            FromDate = source.FromDate,
            ToDate = source.ToDate,
            Sports = source.Sports,
            Locations = source.Locations,
            Fixtures = source.Fixtures,
            Tournaments = source.Tournaments
        };

        public static BaseOutrightRequest Map(GetOutrightMarketsRequestDto source) => new BaseOutrightRequest
        {
            Timestamp = source.Timestamp,
            FromDate = source.FromDate,
            ToDate = source.ToDate,
            Sports = source.Sports,
            Locations = source.Locations,
            Fixtures = source.Fixtures,
            Tournaments = source.Tournaments,
            Markets = source.Markets
        };
    }
}
