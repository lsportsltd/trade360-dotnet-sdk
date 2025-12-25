using AutoMapper;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;

namespace Trade360SDK.CustomersApi.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetLeaguesRequestDto, GetLeaguesRequest>();
            CreateMap<GetMarketsRequestDto, GetMarketsRequest>();
            CreateMap<GetTranslationsRequestDto, GetTranslationsRequest>();
            CreateMap<GetCompetitionsRequestDto, GetCompetitionsRequest>();
            CreateMap<FixtureSubscriptionRequestDto, FixtureSubscriptionRequest>();
            CreateMap<LeagueSubscriptionRequestDto, LeagueSubscriptionRequest>();
            CreateMap<GetFixtureScheduleRequestDto, GetFixtureScheduleRequest>();
            CreateMap<ChangeManualSuspensionRequestDto, ChangeManualSuspensionRequest>();
            CreateMap<GetSubscriptionRequestDto, GetSubscriptionRequest>();
            CreateMap<CompetitionSubscriptionRequestDto, CompetitionSubscriptionRequest>();
            CreateMap<GetFixtureMetadataRequestDto, GetFixtureMetadataRequest>()
                  .ForMember(dest => dest.FromDate, opt => opt.MapFrom(src => src.FromDate.ToString("MM/dd/yyyy")))
                  .ForMember(dest => dest.ToDate, opt => opt.MapFrom(src => src.ToDate.ToString("MM/dd/yyyy")));
            CreateMap<IncidentFilterDto, IncidentFilter>();
            CreateMap<GetIncidentsRequestDto, GetIncidentsRequest>();
            
            CreateMap<GetVenuesRequestDto, GetVenuesRequest>();
            CreateMap<VenueFilterDto, VenueFilter>();
            CreateMap<GetCitiesRequestDto, GetCitiesRequest>();
            CreateMap<CityFilterDto, CityFilter>();
            CreateMap<GetStatesRequestDto, GetStatesRequest>();
            CreateMap<StateFilterDto, StateFilter>();
            CreateMap<GetParticipantsRequestDto, GetParticipantsRequest>();
            CreateMap<ParticipantFilterDto, ParticipantFilter>();
            CreateMap<GetSeasonsRequestDto, GetSeasonsRequest>();
            CreateMap<GetToursRequestDto, GetToursRequest>();
        }
    }
}
//2916,