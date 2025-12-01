using System.Collections.Generic;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Responses
{
    public class GetOutrightMarketsResponseTests
    {
        [Fact]
        public void CanAssignAndRetrieveAllProperties()
        {
            var bet = new Bet { Id = 1, Name = "Bet1" };
            var marketResponse = new OutrightMarketResponse { Id = 2, Name = "Market1", Bets = new List<Bet> { bet } };
            var outrightMarkets = new OutrightMarketsResponse { FixtureId = 3, Markets = new List<OutrightMarketResponse> { marketResponse } };
            var response = new GetOutrightMarketsResponse
            {
                Id = 4,
                Name = "MarketsResp",
                Type = 10,
                Events = new List<OutrightMarketsResponse> { outrightMarkets }
            };

            Assert.Equal(4, response.Id);
            Assert.Equal("MarketsResp", response.Name);
            Assert.Equal(10, response.Type);
            Assert.NotNull(response.Events);
            var evt = Assert.Single(response.Events);
            Assert.Equal(3, evt.FixtureId);
            var market = Assert.Single(evt.Markets);
            Assert.Equal(2, market.Id);
            Assert.Equal("Market1", market.Name);
            var betItem = Assert.Single(market.Bets);
            Assert.Equal(1, betItem.Id);
            Assert.Equal("Bet1", betItem.Name);
        }
    }
} 