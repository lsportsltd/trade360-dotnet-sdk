using AutoMapper;
using Trade360SDK.Common.Metadata.Requests;
using Trade360SDK.CustomersApi.MetadataApi.Requests;

namespace Trade360SDK.CustomersApi.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetLeaguesRequestDto, GetLeaguesRequest>();
            CreateMap<GetMarketsRequestDto, GetMarketsRequest>();
        }
    }
}
