using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.MappingsApi.Responses;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.MappingsApi.Responses
{
    public class GetMappingsResponseTests
    {
        [Fact]
        public void GetMappingsResponse_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            var response = new GetMappingsResponse();

            response.Should().NotBeNull();
            response.Mappings.Should().BeNull();
        }

        [Fact]
        public void MappingEntry_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            var entry = new MappingEntry();

            entry.Should().NotBeNull();
            entry.SportId.Should().Be(0);
            entry.LsportsMarketId.Should().Be(0);
            entry.TradeMarketCode.Should().BeNull();
            entry.TradeDataEndpoint.Should().BeNull();
            entry.SelectionSchema.Should().BeNull();
            entry.Periods.Should().BeNull();
            entry.Status.Should().BeNull();
            entry.Version.Should().BeNull();
        }

        [Fact]
        public void SelectionSchema_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            var schema = new SelectionSchema();

            schema.Should().NotBeNull();
            schema.Over.Should().BeNull();
            schema.Under.Should().BeNull();
            schema.LineParam.Should().BeNull();
            schema.Home.Should().BeNull();
            schema.Away.Should().BeNull();
            schema.Draw.Should().BeNull();
        }

        [Fact]
        public void SelectionSchema_SetProperties_ShouldReturnCorrectValues()
        {
            var schema = new SelectionSchema
            {
                Over = "Over",
                Under = "Under",
                LineParam = "Line",
                Home = "Home",
                Away = "Away",
                Draw = "Draw"
            };

            schema.Over.Should().Be("Over");
            schema.Under.Should().Be("Under");
            schema.LineParam.Should().Be("Line");
            schema.Home.Should().Be("Home");
            schema.Away.Should().Be("Away");
            schema.Draw.Should().Be("Draw");
        }
    }
}
