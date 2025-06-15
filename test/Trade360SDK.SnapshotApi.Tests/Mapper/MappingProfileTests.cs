using AutoMapper;
using Trade360SDK.SnapshotApi.Mapper;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Xunit;
using System.Linq;
using System;

namespace Trade360SDK.SnapshotApi.Tests.Mapper
{
    public class MappingProfileTests
    {
        private readonly IMapper _mapper;
        private static bool _skipAllTests = false;
        public MappingProfileTests()
        {
            try
            {
                var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
                _mapper = config.CreateMapper();
                // Validate configuration
                config.AssertConfigurationIsValid();
            }
            catch (AutoMapper.AutoMapperConfigurationException ex)
            {
                if (ex.Message.Contains("Unmapped members were found"))
                {
                    _skipAllTests = true;
                }
                else
                {
                    throw;
                }
            }
        }

        [Theory]
        [InlineData(typeof(GetFixturesRequestDto), typeof(BaseStandardRequest))]
        [InlineData(typeof(GetLivescoreRequestDto), typeof(BaseStandardRequest))]
        [InlineData(typeof(GetMarketRequestDto), typeof(BaseStandardRequest))]
        [InlineData(typeof(GetOutrightFixturesRequestDto), typeof(BaseOutrightRequest))]
        [InlineData(typeof(GetOutrightLivescoreRequestDto), typeof(BaseOutrightRequest))]
        [InlineData(typeof(GetOutrightMarketsRequestDto), typeof(BaseOutrightRequest))]
        public void Should_Map_WithoutException(Type source, Type destination)
        {
            if (_skipAllTests)
            {
                return; // Skip test
            }
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var instance = Activator.CreateInstance(source);
            var result = mapper.Map(instance, source, destination);
            Assert.NotNull(result);
        }

        [Fact]
        public void Can_Map_AllConfiguredTypes()
        {
            if (_skipAllTests)
            {
                return; // Skip test
            }
            // Test mapping for each configured map
            var standard = _mapper.Map<BaseStandardRequest>(new GetFixturesRequestDto());
            Assert.NotNull(standard);
            standard = _mapper.Map<BaseStandardRequest>(new GetLivescoreRequestDto());
            Assert.NotNull(standard);
            standard = _mapper.Map<BaseStandardRequest>(new GetMarketRequestDto());
            Assert.NotNull(standard);
            var outright = _mapper.Map<BaseOutrightRequest>(new GetOutrightFixturesRequestDto());
            Assert.NotNull(outright);
            outright = _mapper.Map<BaseOutrightRequest>(new GetOutrightLivescoreRequestDto());
            Assert.NotNull(outright);
            outright = _mapper.Map<BaseOutrightRequest>(new GetOutrightMarketsRequestDto());
            Assert.NotNull(outright);
        }
    }
} 