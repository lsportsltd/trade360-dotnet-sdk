using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class MarketTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var bets = new List<Bet> { new Bet { Id = 1 } };
            var providerMarkets = new List<ProviderMarket> { new ProviderMarket { Id = 2 } };
            var market = new Market
            {
                Id = 10,
                Name = "MarketName",
                Bets = bets,
                ProviderMarkets = providerMarkets,
                MainLine = "2.5"
            };
            Assert.Equal(10, market.Id);
            Assert.Equal("MarketName", market.Name);
            Assert.Equal(bets, market.Bets);
            Assert.Equal(providerMarkets, market.ProviderMarkets);
            Assert.Equal("2.5", market.MainLine);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var market = new Market();
            Assert.Null(market.Name);
            Assert.Null(market.Bets);
            Assert.Null(market.ProviderMarkets);
            Assert.Null(market.MainLine);
        }

        [Fact]
        public void DeserializeMarketStatusFromJson_MapsStatusProperty()
        {
            var market = JsonSerializer.Deserialize<Market>("{\"Id\":52,\"Name\":\"1X2\",\"Status\":2,\"Bets\":[]}");

            Assert.NotNull(market);
            Assert.Equal(52, market!.Id);
            Assert.Equal("1X2", market.Name);
            Assert.Equal(MarketStatus.Suspended, market.Status);
        }

        [Fact]
        public void DeserializePredictionData_FromJson_MapsMarketAndBetFields()
        {
            const string json = """
                {
                  "Id": 52,
                  "Name": "1X2",
                  "Status": 1,
                  "PredictionData": { "Volume": 20370 },
                  "Bets": [
                    {
                      "Id": 1,
                      "Name": "Home",
                      "PredictionData": {
                        "Volume": 2529.72,
                        "Liquidity": 0,
                        "StartDate": "2026-01-15T10:00:00.000Z",
                        "EndDate": "2026-01-15T12:00:00.000Z"
                      }
                    }
                  ]
                }
                """;

            var market = JsonSerializer.Deserialize<Market>(json);

            Assert.NotNull(market);
            Assert.NotNull(market!.PredictionData);
            Assert.Equal(20370, market.PredictionData!.Volume);
            var bet = market.Bets!.Single();
            Assert.NotNull(bet.PredictionData);
            Assert.Equal(2529.72, bet.PredictionData!.Volume);
            Assert.Equal(0, bet.PredictionData.Liquidity);
            Assert.Equal(new DateTime(2026, 1, 15, 10, 0, 0, DateTimeKind.Utc), bet.PredictionData.StartDate);
            Assert.Equal(new DateTime(2026, 1, 15, 12, 0, 0, DateTimeKind.Utc), bet.PredictionData.EndDate);
        }
    }
}