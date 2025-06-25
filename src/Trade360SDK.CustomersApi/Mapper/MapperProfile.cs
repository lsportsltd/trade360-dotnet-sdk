using AutoMapper;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;

namespace Trade360SDK.CustomersApi.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetLeaguesRequestDto, GetLeaguesRequest>()
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
                
            CreateMap<GetMarketsRequestDto, GetMarketsRequest>()
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
                
            CreateMap<GetTranslationsRequestDto, GetTranslationsRequest>()
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
                
            CreateMap<GetCompetitionsRequestDto, GetCompetitionsRequest>()
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
                
            CreateMap<FixtureSubscriptionRequestDto, FixtureSubscriptionRequest>()
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
                
            CreateMap<LeagueSubscriptionRequestDto, LeagueSubscriptionRequest>()
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
                
            CreateMap<GetFixtureScheduleRequestDto, GetFixtureScheduleRequest>()
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
                
            CreateMap<ChangeManualSuspensionRequestDto, ChangeManualSuspensionRequest>()
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
                
            CreateMap<GetSubscriptionRequestDto, GetSubscriptionRequest>()
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
                
            CreateMap<CompetitionSubscriptionRequestDto, CompetitionSubscriptionRequest>()
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
                
            CreateMap<GetFixtureMetadataRequestDto, GetFixtureMetadataRequest>()
                .ForMember(dest => dest.FromDate, opt => opt.MapFrom(src => src.FromDate.ToString("MM/dd/yyyy")))
                .ForMember(dest => dest.ToDate, opt => opt.MapFrom(src => src.ToDate.ToString("MM/dd/yyyy")))
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
                
            CreateMap<IncidentFilterDto, IncidentFilter>();
            
            CreateMap<GetIncidentsRequestDto, GetIncidentsRequest>()
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}
