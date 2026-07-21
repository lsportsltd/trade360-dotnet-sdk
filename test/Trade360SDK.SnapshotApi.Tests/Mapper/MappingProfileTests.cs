using Trade360SDK.SnapshotApi.Mapper;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests.Mapper
{
    public class MappingProfileTests
    {
        [Fact]
        public void Can_Map_AllConfiguredTypes()
        {
            Assert.NotNull(SnapshotApiMapper.Map(new GetFixturesRequestDto()));
            Assert.NotNull(SnapshotApiMapper.Map(new GetLivescoreRequestDto()));
            Assert.NotNull(SnapshotApiMapper.Map(new GetMarketRequestDto()));
            Assert.NotNull(SnapshotApiMapper.Map(new GetOutrightFixturesRequestDto()));
            Assert.NotNull(SnapshotApiMapper.Map(new GetOutrightLivescoreRequestDto()));
            Assert.NotNull(SnapshotApiMapper.Map(new GetOutrightMarketsRequestDto()));
        }

        [Fact]
        public void Map_GetMarketRequestDto_CopiesMarkets()
        {
            var dto = new GetMarketRequestDto { Markets = new[] { 1, 2 } };
            var result = SnapshotApiMapper.Map(dto);
            Assert.Equal(new[] { 1, 2 }, result.Markets);
        }

        [Fact]
        public void Map_GetOutrightMarketsRequestDto_CopiesMarkets()
        {
            var dto = new GetOutrightMarketsRequestDto { Markets = new[] { 9 } };
            var result = SnapshotApiMapper.Map(dto);
            Assert.Equal(new[] { 9 }, result.Markets);
        }
    }
}
