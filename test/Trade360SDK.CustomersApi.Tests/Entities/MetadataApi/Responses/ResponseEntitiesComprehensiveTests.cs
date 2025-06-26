using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Responses
{
    public class ResponseEntitiesComprehensiveTests
    {
        #region Market Entity Tests

        [Fact]
        public void Market_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var market = new Market();

            // Assert
            market.Should().NotBeNull();
            market.Id.Should().Be(0);
            market.Name.Should().BeNull();
            market.IsSettleable.Should().BeFalse();
        }

        [Fact]
        public void Market_Properties_ShouldBeSettableAndGettable()
        {
            // Arrange
            var market = new Market();
            var expectedId = 123;
            var expectedName = "Match Winner";
            var expectedIsSettleable = true;

            // Act
            market.Id = expectedId;
            market.Name = expectedName;
            market.IsSettleable = expectedIsSettleable;

            // Assert
            market.Id.Should().Be(expectedId);
            market.Name.Should().Be(expectedName);
            market.IsSettleable.Should().Be(expectedIsSettleable);
        }

        [Theory]
        [InlineData(1, "Over/Under", true)]
        [InlineData(2, "Both Teams to Score", false)]
        [InlineData(999, "Asian Handicap", true)]
        [InlineData(0, "", false)]
        [InlineData(-1, null, true)]
        public void Market_WithVariousValues_ShouldStoreCorrectly(int id, string name, bool isSettleable)
        {
            // Act
            var market = new Market
            {
                Id = id,
                Name = name,
                IsSettleable = isSettleable
            };

            // Assert
            market.Id.Should().Be(id);
            market.Name.Should().Be(name);
            market.IsSettleable.Should().Be(isSettleable);
        }

        [Fact]
        public void Market_WithSpecialCharactersInName_ShouldHandleCorrectly()
        {
            // Arrange
            var market = new Market();
            var specialName = "Over/Under 2.5 Goals & Both Teams to Score";

            // Act
            market.Name = specialName;

            // Assert
            market.Name.Should().Be(specialName);
        }

        [Fact]
        public void Market_WithUnicodeCharacters_ShouldHandleCorrectly()
        {
            // Arrange
            var market = new Market();
            var unicodeName = "胜负平 (1X2)";

            // Act
            market.Name = unicodeName;

            // Assert
            market.Name.Should().Be(unicodeName);
        }

        #endregion

        #region MarketsCollectionResponse Tests

        [Fact]
        public void MarketsCollectionResponse_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var response = new MarketsCollectionResponse();

            // Assert
            response.Should().NotBeNull();
            response.Markets.Should().BeNull();
        }

        [Fact]
        public void MarketsCollectionResponse_WithEmptyCollection_ShouldStoreCorrectly()
        {
            // Arrange
            var emptyMarkets = new List<Market>();

            // Act
            var response = new MarketsCollectionResponse
            {
                Markets = emptyMarkets
            };

            // Assert
            response.Markets.Should().NotBeNull();
            response.Markets.Should().BeEmpty();
        }

        [Fact]
        public void MarketsCollectionResponse_WithMultipleMarkets_ShouldStoreCorrectly()
        {
            // Arrange
            var markets = new List<Market>
            {
                new Market { Id = 1, Name = "Match Winner", IsSettleable = true },
                new Market { Id = 2, Name = "Over/Under", IsSettleable = false },
                new Market { Id = 3, Name = "Asian Handicap", IsSettleable = true }
            };

            // Act
            var response = new MarketsCollectionResponse
            {
                Markets = markets
            };

            // Assert
            response.Markets.Should().NotBeNull();
            response.Markets.Should().HaveCount(3);
            response.Markets.Should().Contain(m => m.Name == "Match Winner");
            response.Markets.Should().Contain(m => m.Name == "Over/Under");
            response.Markets.Should().Contain(m => m.Name == "Asian Handicap");
        }

        [Fact]
        public void MarketsCollectionResponse_WithNullMarkets_ShouldStoreNull()
        {
            // Act
            var response = new MarketsCollectionResponse
            {
                Markets = null
            };

            // Assert
            response.Markets.Should().BeNull();
        }

        #endregion

        #region GetDistributionStatusResponse Tests

        [Fact]
        public void GetDistributionStatusResponse_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var response = new GetDistributionStatusResponse();

            // Assert
            response.Should().NotBeNull();
            response.IsDistributionOn.Should().BeFalse();
            response.Consumers.Should().BeNull();
            response.NumberMessagesInQueue.Should().Be(0);
            response.MessagesPerSecond.Should().Be(0.0);
        }

        [Fact]
        public void GetDistributionStatusResponse_Properties_ShouldBeSettableAndGettable()
        {
            // Arrange
            var response = new GetDistributionStatusResponse();
            var expectedIsDistributionOn = true;
            var expectedConsumers = new[] { "consumer1", "consumer2" };
            var expectedNumberMessages = 150;
            var expectedMessagesPerSecond = 25.5;

            // Act
            response.IsDistributionOn = expectedIsDistributionOn;
            response.Consumers = expectedConsumers;
            response.NumberMessagesInQueue = expectedNumberMessages;
            response.MessagesPerSecond = expectedMessagesPerSecond;

            // Assert
            response.IsDistributionOn.Should().Be(expectedIsDistributionOn);
            response.Consumers.Should().BeEquivalentTo(expectedConsumers);
            response.NumberMessagesInQueue.Should().Be(expectedNumberMessages);
            response.MessagesPerSecond.Should().Be(expectedMessagesPerSecond);
        }

        [Theory]
        [InlineData(true, 100, 50.5)]
        [InlineData(false, 0, 0.0)]
        [InlineData(true, 999999, 999.999)]
        [InlineData(false, -1, -1.0)]
        public void GetDistributionStatusResponse_WithVariousValues_ShouldStoreCorrectly(bool isOn, int messageCount, double messagesPerSecond)
        {
            // Act
            var response = new GetDistributionStatusResponse
            {
                IsDistributionOn = isOn,
                NumberMessagesInQueue = messageCount,
                MessagesPerSecond = messagesPerSecond
            };

            // Assert
            response.IsDistributionOn.Should().Be(isOn);
            response.NumberMessagesInQueue.Should().Be(messageCount);
            response.MessagesPerSecond.Should().Be(messagesPerSecond);
        }

        [Fact]
        public void GetDistributionStatusResponse_WithEmptyConsumers_ShouldStoreCorrectly()
        {
            // Arrange
            var emptyConsumers = new string[0];

            // Act
            var response = new GetDistributionStatusResponse
            {
                Consumers = emptyConsumers
            };

            // Assert
            response.Consumers.Should().NotBeNull();
            response.Consumers.Should().BeEmpty();
        }

        [Fact]
        public void GetDistributionStatusResponse_WithMultipleConsumers_ShouldStoreCorrectly()
        {
            // Arrange
            var consumers = new[] { "consumer1", "consumer2", "consumer3", "consumer4" };

            // Act
            var response = new GetDistributionStatusResponse
            {
                Consumers = consumers
            };

            // Assert
            response.Consumers.Should().NotBeNull();
            response.Consumers.Should().HaveCount(4);
            response.Consumers.Should().Contain("consumer1");
            response.Consumers.Should().Contain("consumer2");
            response.Consumers.Should().Contain("consumer3");
            response.Consumers.Should().Contain("consumer4");
        }

        [Fact]
        public void GetDistributionStatusResponse_WithNullConsumers_ShouldStoreNull()
        {
            // Act
            var response = new GetDistributionStatusResponse
            {
                Consumers = null
            };

            // Assert
            response.Consumers.Should().BeNull();
        }

        [Fact]
        public void GetDistributionStatusResponse_WithSpecialCharactersInConsumerNames_ShouldHandleCorrectly()
        {
            // Arrange
            var specialConsumers = new[] { "consumer@domain.com", "consumer-with-dash", "consumer_with_underscore" };

            // Act
            var response = new GetDistributionStatusResponse
            {
                Consumers = specialConsumers
            };

            // Assert
            response.Consumers.Should().BeEquivalentTo(specialConsumers);
        }

        #endregion

        #region StartDistributionResponse Tests

        [Fact]
        public void StartDistributionResponse_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var response = new StartDistributionResponse();

            // Assert
            response.Should().NotBeNull();
            response.Message.Should().BeNull();
        }

        [Fact]
        public void StartDistributionResponse_Message_ShouldBeSettableAndGettable()
        {
            // Arrange
            var response = new StartDistributionResponse();
            var expectedMessage = "Distribution started successfully";

            // Act
            response.Message = expectedMessage;

            // Assert
            response.Message.Should().Be(expectedMessage);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Success")]
        [InlineData("Distribution started successfully")]
        [InlineData("Error: Unable to start distribution")]
        [InlineData("Very long message that exceeds normal length limits and contains detailed information about the distribution start process")]
        public void StartDistributionResponse_WithVariousMessages_ShouldStoreCorrectly(string message)
        {
            // Act
            var response = new StartDistributionResponse
            {
                Message = message
            };

            // Assert
            response.Message.Should().Be(message);
        }

        [Fact]
        public void StartDistributionResponse_WithNullMessage_ShouldStoreNull()
        {
            // Act
            var response = new StartDistributionResponse
            {
                Message = null
            };

            // Assert
            response.Message.Should().BeNull();
        }

        #endregion

        #region StopDistributionResponse Tests

        [Fact]
        public void StopDistributionResponse_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var response = new StopDistributionResponse();

            // Assert
            response.Should().NotBeNull();
            response.Message.Should().BeNull();
        }

        [Fact]
        public void StopDistributionResponse_Message_ShouldBeSettableAndGettable()
        {
            // Arrange
            var response = new StopDistributionResponse();
            var expectedMessage = "Distribution stopped successfully";

            // Act
            response.Message = expectedMessage;

            // Assert
            response.Message.Should().Be(expectedMessage);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Success")]
        [InlineData("Distribution stopped successfully")]
        [InlineData("Error: Unable to stop distribution")]
        [InlineData("Distribution stopped with warnings: Some consumers may still be active")]
        public void StopDistributionResponse_WithVariousMessages_ShouldStoreCorrectly(string message)
        {
            // Act
            var response = new StopDistributionResponse
            {
                Message = message
            };

            // Assert
            response.Message.Should().Be(message);
        }

        [Fact]
        public void StopDistributionResponse_WithNullMessage_ShouldStoreNull()
        {
            // Act
            var response = new StopDistributionResponse
            {
                Message = null
            };

            // Assert
            response.Message.Should().BeNull();
        }

        #endregion

        #region Cross-Entity Integration Tests

        [Fact]
        public void ResponseEntities_ShouldBeIndependent()
        {
            // Arrange & Act
            var market = new Market { Id = 1, Name = "Test Market" };
            var distributionStatus = new GetDistributionStatusResponse { IsDistributionOn = true };
            var startResponse = new StartDistributionResponse { Message = "Started" };
            var stopResponse = new StopDistributionResponse { Message = "Stopped" };

            // Assert
            market.Should().NotBeSameAs(distributionStatus);
            distributionStatus.Should().NotBeSameAs(startResponse);
            startResponse.Should().NotBeSameAs(stopResponse);

            // Verify each entity maintains its own state
            market.Id.Should().Be(1);
            distributionStatus.IsDistributionOn.Should().BeTrue();
            startResponse.Message.Should().Be("Started");
            stopResponse.Message.Should().Be("Stopped");
        }

        [Fact]
        public void MarketsCollectionResponse_WithComplexMarketData_ShouldHandleCorrectly()
        {
            // Arrange
            var complexMarkets = new List<Market>
            {
                new Market { Id = 1, Name = "1X2", IsSettleable = true },
                new Market { Id = 2, Name = "Over/Under 0.5", IsSettleable = false },
                new Market { Id = 3, Name = "Asian Handicap +1.5", IsSettleable = true },
                new Market { Id = 4, Name = "Both Teams to Score & Over 2.5", IsSettleable = false },
                new Market { Id = 5, Name = "Correct Score", IsSettleable = true }
            };

            // Act
            var response = new MarketsCollectionResponse
            {
                Markets = complexMarkets
            };

            // Assert
            response.Markets.Should().HaveCount(5);
            response.Markets.Count(m => m.IsSettleable).Should().Be(3);
            response.Markets.Count(m => !m.IsSettleable).Should().Be(2);
            response.Markets.Should().Contain(m => m.Name.Contains("Asian Handicap"));
            response.Markets.Should().Contain(m => m.Name.Contains("Both Teams to Score"));
        }

        #endregion

        #region Edge Cases and Error Handling

        [Fact]
        public void ResponseEntities_WithExtremeValues_ShouldHandleCorrectly()
        {
            // Arrange & Act
            var market = new Market
            {
                Id = int.MaxValue,
                Name = new string('A', 10000),
                IsSettleable = true
            };

            var distributionStatus = new GetDistributionStatusResponse
            {
                NumberMessagesInQueue = int.MaxValue,
                MessagesPerSecond = double.MaxValue,
                IsDistributionOn = true
            };

            // Assert
            market.Id.Should().Be(int.MaxValue);
            market.Name.Should().HaveLength(10000);
            distributionStatus.NumberMessagesInQueue.Should().Be(int.MaxValue);
            distributionStatus.MessagesPerSecond.Should().Be(double.MaxValue);
        }

        [Fact]
        public void ResponseEntities_WithMinimumValues_ShouldHandleCorrectly()
        {
            // Arrange & Act
            var market = new Market
            {
                Id = int.MinValue,
                Name = "",
                IsSettleable = false
            };

            var distributionStatus = new GetDistributionStatusResponse
            {
                NumberMessagesInQueue = int.MinValue,
                MessagesPerSecond = double.MinValue,
                IsDistributionOn = false
            };

            // Assert
            market.Id.Should().Be(int.MinValue);
            market.Name.Should().BeEmpty();
            distributionStatus.NumberMessagesInQueue.Should().Be(int.MinValue);
            distributionStatus.MessagesPerSecond.Should().Be(double.MinValue);
        }

        #endregion

        #region Type Safety Tests

        [Fact]
        public void ResponseEntities_ShouldHaveCorrectTypes()
        {
            // Act
            var market = new Market();
            var marketsCollection = new MarketsCollectionResponse();
            var distributionStatus = new GetDistributionStatusResponse();
            var startResponse = new StartDistributionResponse();
            var stopResponse = new StopDistributionResponse();

            // Assert
            market.Should().BeOfType<Market>();
            marketsCollection.Should().BeOfType<MarketsCollectionResponse>();
            distributionStatus.Should().BeOfType<GetDistributionStatusResponse>();
            startResponse.Should().BeOfType<StartDistributionResponse>();
            stopResponse.Should().BeOfType<StopDistributionResponse>();
        }

        #endregion

        #region Business Logic Tests

        [Fact]
        public void Market_WithValidMarketData_ShouldRepresentValidState()
        {
            // Arrange
            var market = new Market
            {
                Id = 123,
                Name = "Match Winner",
                IsSettleable = true
            };

            // Act & Assert
            market.Id.Should().BeGreaterThan(0);
            market.Name.Should().NotBeNullOrEmpty();
            market.IsSettleable.Should().BeTrue();
        }

        [Fact]
        public void GetDistributionStatusResponse_WithActiveDistribution_ShouldRepresentActiveState()
        {
            // Arrange
            var response = new GetDistributionStatusResponse
            {
                IsDistributionOn = true,
                Consumers = new[] { "consumer1", "consumer2" },
                NumberMessagesInQueue = 100,
                MessagesPerSecond = 50.5
            };

            // Act & Assert
            response.IsDistributionOn.Should().BeTrue();
            response.Consumers.Should().NotBeEmpty();
            response.NumberMessagesInQueue.Should().BeGreaterThan(0);
            response.MessagesPerSecond.Should().BeGreaterThan(0);
        }

        [Fact]
        public void GetDistributionStatusResponse_WithInactiveDistribution_ShouldRepresentInactiveState()
        {
            // Arrange
            var response = new GetDistributionStatusResponse
            {
                IsDistributionOn = false,
                Consumers = new string[0],
                NumberMessagesInQueue = 0,
                MessagesPerSecond = 0.0
            };

            // Act & Assert
            response.IsDistributionOn.Should().BeFalse();
            response.Consumers.Should().BeEmpty();
            response.NumberMessagesInQueue.Should().Be(0);
            response.MessagesPerSecond.Should().Be(0.0);
        }

        #endregion

        #region Performance Tests

        [Fact]
        public void MarketsCollectionResponse_WithLargeNumberOfMarkets_ShouldHandleEfficiently()
        {
            // Arrange
            var largeMarketList = new List<Market>();
            for (int i = 1; i <= 10000; i++)
            {
                largeMarketList.Add(new Market
                {
                    Id = i,
                    Name = $"Market {i}",
                    IsSettleable = i % 2 == 0
                });
            }

            // Act
            var response = new MarketsCollectionResponse
            {
                Markets = largeMarketList
            };

            // Assert
            response.Markets.Should().HaveCount(10000);
            response.Markets.Count(m => m.IsSettleable).Should().Be(5000);
            response.Markets.First().Id.Should().Be(1);
            response.Markets.Last().Id.Should().Be(10000);
        }

        [Fact]
        public void GetDistributionStatusResponse_WithLargeConsumerArray_ShouldHandleEfficiently()
        {
            // Arrange
            var largeConsumerArray = new string[1000];
            for (int i = 0; i < 1000; i++)
            {
                largeConsumerArray[i] = $"consumer{i}";
            }

            // Act
            var response = new GetDistributionStatusResponse
            {
                Consumers = largeConsumerArray
            };

            // Assert
            response.Consumers.Should().HaveCount(1000);
            response.Consumers.First().Should().Be("consumer0");
            response.Consumers.Last().Should().Be("consumer999");
        }

        #endregion
    }
} 