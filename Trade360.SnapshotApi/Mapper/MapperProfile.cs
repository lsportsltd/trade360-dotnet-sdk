﻿using AutoMapper;
using Trade360SDK.SnapshotApi.Entities.Requests;

namespace Trade360SDK.CustomersApi.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetFixturesRequestDto, BaseStandardRequest>();
            CreateMap<GetLivescoreRequestDto, BaseStandardRequest>();
            CreateMap<GetMarketRequestDto, BaseStandardRequest>();
        }
    }
}
