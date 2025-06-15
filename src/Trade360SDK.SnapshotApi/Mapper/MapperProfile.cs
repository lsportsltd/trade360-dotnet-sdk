using AutoMapper;
using Trade360SDK.SnapshotApi.Entities.Requests;

namespace Trade360SDK.SnapshotApi.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetFixturesRequestDto, BaseStandardRequest>()
                .ForMember(dest => dest.Markets, opt => opt.Ignore())
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<GetLivescoreRequestDto, BaseStandardRequest>()
                .ForMember(dest => dest.Markets, opt => opt.Ignore())
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<GetMarketRequestDto, BaseStandardRequest>()
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<GetOutrightFixturesRequestDto, BaseOutrightRequest>()
                .ForMember(dest => dest.Markets, opt => opt.Ignore())
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<GetOutrightLivescoreRequestDto, BaseOutrightRequest>()
                .ForMember(dest => dest.Markets, opt => opt.Ignore())
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<GetOutrightMarketsRequestDto, BaseOutrightRequest>()
                .ForMember(dest => dest.PackageId, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}
