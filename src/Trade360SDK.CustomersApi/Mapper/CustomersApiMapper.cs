using System.Globalization;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;

namespace Trade360SDK.CustomersApi.Mapper
{
    public static class CustomersApiMapper
    {
        public static GetLeaguesRequest Map(GetLeaguesRequestDto source) => new GetLeaguesRequest
        {
            SportIds = source.SportIds,
            LocationIds = source.LocationIds,
            SubscriptionStatus = source.SubscriptionStatus,
            LanguageId = source.LanguageId
        };

        public static GetMarketsRequest Map(GetMarketsRequestDto source) => new GetMarketsRequest
        {
            SportIds = source.SportIds,
            LocationIds = source.LocationIds,
            LeagueIds = source.LeagueIds,
            MarketIds = source.MarketIds,
            IsSettleable = source.IsSettleable,
            MarketType = source.MarketType,
            LanguageId = source.LanguageId
        };

        public static GetTranslationsRequest Map(GetTranslationsRequestDto source) => new GetTranslationsRequest
        {
            SportIds = source.SportIds,
            LocationIds = source.LocationIds,
            LeagueIds = source.LeagueIds,
            MarketIds = source.MarketIds,
            ParticipantIds = source.ParticipantIds,
            Languages = source.Languages,
            LanguageId = source.LanguageId
        };

        public static GetCompetitionsRequest Map(GetCompetitionsRequestDto source) => new GetCompetitionsRequest
        {
            SportIds = source.SportIds,
            LocationIds = source.LocationIds,
            SubscriptionStatus = source.SubscriptionStatus,
            LanguageId = source.LanguageId
        };

        public static FixtureSubscriptionRequest Map(FixtureSubscriptionRequestDto source) => new FixtureSubscriptionRequest
        {
            Fixtures = source.Fixtures
        };

        public static LeagueSubscriptionRequest Map(LeagueSubscriptionRequestDto source) => new LeagueSubscriptionRequest
        {
            Subscriptions = source.Subscriptions
        };

        public static GetFixtureScheduleRequest Map(GetFixtureScheduleRequestDto source) => new GetFixtureScheduleRequest
        {
            SportIds = source.SportIds,
            LocationIds = source.LocationIds,
            LeagueIds = source.LeagueIds
        };

        public static ChangeManualSuspensionRequest Map(ChangeManualSuspensionRequestDto source) => new ChangeManualSuspensionRequest
        {
            Suspensions = source.Suspensions
        };

        public static GetSubscriptionRequest Map(GetSubscriptionRequestDto source) => new GetSubscriptionRequest
        {
            SportIds = source.SportIds,
            LocationIds = source.LocationIds,
            LeagueIds = source.LeagueIds
        };

        public static CompetitionSubscriptionRequest Map(CompetitionSubscriptionRequestDto source) => new CompetitionSubscriptionRequest
        {
            Subscriptions = source.Subscriptions
        };

        public static GetFixtureMetadataRequest Map(GetFixtureMetadataRequestDto source) => new GetFixtureMetadataRequest
        {
            FromDate = source.FromDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
            ToDate = source.ToDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
            SportIds = source.SportIds,
            LocationIds = source.LocationIds,
            LeagueIds = source.LeagueIds
        };

        public static IncidentFilter Map(IncidentFilterDto source) => new IncidentFilter
        {
            Ids = source.Ids,
            Sports = source.Sports,
            From = source.From,
            SearchText = source.SearchText
        };

        public static GetIncidentsRequest Map(GetIncidentsRequestDto source) => new GetIncidentsRequest
        {
            Filter = source.Filter == null ? null : Map(source.Filter)
        };

        public static VenueFilter Map(VenueFilterDto source) => new VenueFilter
        {
            VenueIds = source.VenueIds,
            CountryIds = source.CountryIds,
            StateIds = source.StateIds,
            CityIds = source.CityIds
        };

        public static GetVenuesRequest Map(GetVenuesRequestDto source) => new GetVenuesRequest
        {
            Filter = source.Filter == null ? null : Map(source.Filter)
        };

        public static CityFilter Map(CityFilterDto source) => new CityFilter
        {
            CountryIds = source.CountryIds,
            StateIds = source.StateIds,
            CityIds = source.CityIds
        };

        public static GetCitiesRequest Map(GetCitiesRequestDto source) => new GetCitiesRequest
        {
            Filter = source.Filter == null ? null : Map(source.Filter)
        };

        public static StateFilter Map(StateFilterDto source) => new StateFilter
        {
            CountryIds = source.CountryIds,
            StateIds = source.StateIds
        };

        public static GetStatesRequest Map(GetStatesRequestDto source) => new GetStatesRequest
        {
            Filter = source.Filter == null ? null : Map(source.Filter)
        };

        public static ParticipantFilter Map(ParticipantFilterDto source) => new ParticipantFilter
        {
            Ids = source.Ids,
            SportIds = source.SportIds,
            LocationIds = source.LocationIds,
            Name = source.Name,
            Gender = source.Gender,
            AgeCategory = source.AgeCategory,
            Type = source.Type
        };

        public static GetParticipantsRequest Map(GetParticipantsRequestDto source) => new GetParticipantsRequest
        {
            Filter = source.Filter == null ? null : Map(source.Filter),
            Page = source.Page,
            PageSize = source.PageSize
        };

        public static GetSeasonsRequest Map(GetSeasonsRequestDto source) => new GetSeasonsRequest
        {
            SeasonId = source.SeasonId
        };

        public static GetToursRequest Map(GetToursRequestDto source) => new GetToursRequest
        {
            TourId = source.TourId,
            SportId = source.SportId
        };
    }
}
