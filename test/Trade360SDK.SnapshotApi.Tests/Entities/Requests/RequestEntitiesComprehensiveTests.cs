using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Requests
{
    public class RequestEntitiesComprehensiveTests
    {
        #region BaseRequest Tests

        [Fact]
        public void BaseRequest_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var request = new BaseRequest();

            // Assert
            request.Should().NotBeNull();
            request.PackageId.Should().Be(0);
            request.UserName.Should().BeNull();
            request.Password.Should().BeNull();
        }

        [Fact]
        public void BaseRequest_PackageId_CanBeSetAndRetrieved()
        {
            // Arrange
            var request = new BaseRequest();
            const int expectedPackageId = 12345;

            // Act
            request.PackageId = expectedPackageId;

            // Assert
            request.PackageId.Should().Be(expectedPackageId);
        }

        [Fact]
        public void BaseRequest_UserName_CanBeSetAndRetrieved()
        {
            // Arrange
            var request = new BaseRequest();
            const string expectedUserName = "testuser123";

            // Act
            request.UserName = expectedUserName;

            // Assert
            request.UserName.Should().Be(expectedUserName);
        }

        [Fact]
        public void BaseRequest_Password_CanBeSetAndRetrieved()
        {
            // Arrange
            var request = new BaseRequest();
            const string expectedPassword = "securepassword";

            // Act
            request.Password = expectedPassword;

            // Assert
            request.Password.Should().Be(expectedPassword);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(999999)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void BaseRequest_PackageId_ShouldAcceptVariousValues(int packageId)
        {
            // Arrange
            var request = new BaseRequest();

            // Act
            request.PackageId = packageId;

            // Assert
            request.PackageId.Should().Be(packageId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("user")]
        [InlineData("user@domain.com")]
        [InlineData("very_long_username_with_special_chars_!@#$%")]
        [InlineData("用户名")]
        public void BaseRequest_UserName_ShouldAcceptVariousStrings(string userName)
        {
            // Arrange
            var request = new BaseRequest();

            // Act
            request.UserName = userName;

            // Assert
            request.UserName.Should().Be(userName);
        }

        [Fact]
        public void BaseRequest_UserName_CanBeSetToNull()
        {
            // Arrange
            var request = new BaseRequest { UserName = "initial" };

            // Act
            request.UserName = null;

            // Assert
            request.UserName.Should().BeNull();
        }

        [Fact]
        public void BaseRequest_Password_CanBeSetToNull()
        {
            // Arrange
            var request = new BaseRequest { Password = "initial" };

            // Act
            request.Password = null;

            // Assert
            request.Password.Should().BeNull();
        }

        [Fact]
        public void BaseRequest_Properties_ShouldHaveCorrectTypes()
        {
            // Arrange
            var request = new BaseRequest();

            // Act & Assert
            request.PackageId.Should().BeOfType(typeof(int));
            
            // UserName and Password can be null, so we test after assignment
            request.UserName = "test";
            request.Password = "test";
            
            request.UserName.Should().BeOfType(typeof(string));
            request.Password.Should().BeOfType(typeof(string));
        }

        #endregion

        #region GetMarketRequestDto Tests

        [Fact]
        public void GetMarketRequestDto_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var request = new GetMarketRequestDto();

            // Assert
            request.Should().NotBeNull();
            request.Timestamp.Should().BeNull();
            request.FromDate.Should().BeNull();
            request.ToDate.Should().BeNull();
            request.Sports.Should().NotBeNull().And.BeEmpty();
            request.Locations.Should().NotBeNull().And.BeEmpty();
            request.Fixtures.Should().NotBeNull().And.BeEmpty();
            request.Leagues.Should().NotBeNull().And.BeEmpty();
            request.Markets.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void GetMarketRequestDto_Timestamp_CanBeSetAndRetrieved()
        {
            // Arrange
            var request = new GetMarketRequestDto();
            const long expectedTimestamp = 1234567890L;

            // Act
            request.Timestamp = expectedTimestamp;

            // Assert
            request.Timestamp.Should().Be(expectedTimestamp);
        }

        [Fact]
        public void GetMarketRequestDto_FromDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var request = new GetMarketRequestDto();
            const long expectedFromDate = 1609459200L; // 2021-01-01

            // Act
            request.FromDate = expectedFromDate;

            // Assert
            request.FromDate.Should().Be(expectedFromDate);
        }

        [Fact]
        public void GetMarketRequestDto_ToDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var request = new GetMarketRequestDto();
            const long expectedToDate = 1640995200L; // 2022-01-01

            // Act
            request.ToDate = expectedToDate;

            // Assert
            request.ToDate.Should().Be(expectedToDate);
        }

        [Fact]
        public void GetMarketRequestDto_Sports_CanBeSetAndRetrieved()
        {
            // Arrange
            var request = new GetMarketRequestDto();
            var expectedSports = new[] { 1, 2, 3, 4, 5 };

            // Act
            request.Sports = expectedSports;

            // Assert
            request.Sports.Should().Equal(expectedSports);
        }

        [Fact]
        public void GetMarketRequestDto_Locations_CanBeSetAndRetrieved()
        {
            // Arrange
            var request = new GetMarketRequestDto();
            var expectedLocations = new[] { 10, 20, 30 };

            // Act
            request.Locations = expectedLocations;

            // Assert
            request.Locations.Should().Equal(expectedLocations);
        }

        [Fact]
        public void GetMarketRequestDto_Fixtures_CanBeSetAndRetrieved()
        {
            // Arrange
            var request = new GetMarketRequestDto();
            var expectedFixtures = new[] { 100, 200, 300 };

            // Act
            request.Fixtures = expectedFixtures;

            // Assert
            request.Fixtures.Should().Equal(expectedFixtures);
        }

        [Fact]
        public void GetMarketRequestDto_Leagues_CanBeSetAndRetrieved()
        {
            // Arrange
            var request = new GetMarketRequestDto();
            var expectedLeagues = new[] { 1000, 2000, 3000 };

            // Act
            request.Leagues = expectedLeagues;

            // Assert
            request.Leagues.Should().Equal(expectedLeagues);
        }

        [Fact]
        public void GetMarketRequestDto_Markets_CanBeSetAndRetrieved()
        {
            // Arrange
            var request = new GetMarketRequestDto();
            var expectedMarkets = new[] { 10000, 20000, 30000 };

            // Act
            request.Markets = expectedMarkets;

            // Assert
            request.Markets.Should().Equal(expectedMarkets);
        }

        [Fact]
        public void GetMarketRequestDto_Collections_CanBeSetToEmpty()
        {
            // Arrange
            var request = new GetMarketRequestDto();

            // Act
            request.Sports = Enumerable.Empty<int>();
            request.Locations = Enumerable.Empty<int>();
            request.Fixtures = Enumerable.Empty<int>();
            request.Leagues = Enumerable.Empty<int>();
            request.Markets = Enumerable.Empty<int>();

            // Assert
            request.Sports.Should().BeEmpty();
            request.Locations.Should().BeEmpty();
            request.Fixtures.Should().BeEmpty();
            request.Leagues.Should().BeEmpty();
            request.Markets.Should().BeEmpty();
        }

        [Fact]
        public void GetMarketRequestDto_Collections_CanContainSingleValue()
        {
            // Arrange
            var request = new GetMarketRequestDto();

            // Act
            request.Sports = new[] { 1 };
            request.Locations = new[] { 10 };
            request.Fixtures = new[] { 100 };
            request.Leagues = new[] { 1000 };
            request.Markets = new[] { 10000 };

            // Assert
            request.Sports.Should().HaveCount(1).And.Contain(1);
            request.Locations.Should().HaveCount(1).And.Contain(10);
            request.Fixtures.Should().HaveCount(1).And.Contain(100);
            request.Leagues.Should().HaveCount(1).And.Contain(1000);
            request.Markets.Should().HaveCount(1).And.Contain(10000);
        }

        [Fact]
        public void GetMarketRequestDto_Collections_CanContainMultipleValues()
        {
            // Arrange
            var request = new GetMarketRequestDto();

            // Act
            request.Sports = new[] { 1, 2, 3, 4, 5 };
            request.Locations = new[] { 10, 20, 30, 40, 50 };
            request.Fixtures = new[] { 100, 200, 300, 400, 500 };
            request.Leagues = new[] { 1000, 2000, 3000, 4000, 5000 };
            request.Markets = new[] { 10000, 20000, 30000, 40000, 50000 };

            // Assert
            request.Sports.Should().HaveCount(5);
            request.Locations.Should().HaveCount(5);
            request.Fixtures.Should().HaveCount(5);
            request.Leagues.Should().HaveCount(5);
            request.Markets.Should().HaveCount(5);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0L)]
        [InlineData(1L)]
        [InlineData(1234567890L)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public void GetMarketRequestDto_TimestampValues_ShouldBeAccepted(long? timestamp)
        {
            // Arrange
            var request = new GetMarketRequestDto();

            // Act
            request.Timestamp = timestamp;

            // Assert
            request.Timestamp.Should().Be(timestamp);
        }

        [Fact]
        public void GetMarketRequestDto_Collections_CanContainDuplicateValues()
        {
            // Arrange
            var request = new GetMarketRequestDto();

            // Act
            request.Sports = new[] { 1, 1, 2, 2, 3, 3 };
            request.Locations = new[] { 10, 10, 20, 20 };

            // Assert
            request.Sports.Should().HaveCount(6);
            request.Locations.Should().HaveCount(4);
        }

        [Fact]
        public void GetMarketRequestDto_Collections_CanContainNegativeValues()
        {
            // Arrange
            var request = new GetMarketRequestDto();

            // Act
            request.Sports = new[] { -1, -2, -3 };
            request.Locations = new[] { -10, -20, -30 };

            // Assert
            request.Sports.Should().Equal(-1, -2, -3);
            request.Locations.Should().Equal(-10, -20, -30);
        }

        #endregion

        #region GetOutrightMarketsRequestDto Tests

        [Fact]
        public void GetOutrightMarketsRequestDto_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var request = new GetOutrightMarketsRequestDto();

            // Assert
            request.Should().NotBeNull();
            request.Timestamp.Should().BeNull();
            request.FromDate.Should().BeNull();
            request.ToDate.Should().BeNull();
            request.Sports.Should().NotBeNull().And.BeEmpty();
            request.Locations.Should().NotBeNull().And.BeEmpty();
            request.Fixtures.Should().NotBeNull().And.BeEmpty();
            request.Tournaments.Should().NotBeNull().And.BeEmpty();
            request.Markets.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void GetOutrightMarketsRequestDto_Tournaments_CanBeSetAndRetrieved()
        {
            // Arrange
            var request = new GetOutrightMarketsRequestDto();
            var expectedTournaments = new[] { 1001, 2002, 3003 };

            // Act
            request.Tournaments = expectedTournaments;

            // Assert
            request.Tournaments.Should().Equal(expectedTournaments);
        }

        [Fact]
        public void GetOutrightMarketsRequestDto_AllProperties_CanBeSetSimultaneously()
        {
            // Arrange
            var request = new GetOutrightMarketsRequestDto();
            const long timestamp = 1234567890L;
            const long fromDate = 1609459200L;
            const long toDate = 1640995200L;
            var sports = new[] { 1, 2 };
            var locations = new[] { 10, 20 };
            var fixtures = new[] { 100, 200 };
            var tournaments = new[] { 1000, 2000 };
            var markets = new[] { 10000, 20000 };

            // Act
            request.Timestamp = timestamp;
            request.FromDate = fromDate;
            request.ToDate = toDate;
            request.Sports = sports;
            request.Locations = locations;
            request.Fixtures = fixtures;
            request.Tournaments = tournaments;
            request.Markets = markets;

            // Assert
            request.Timestamp.Should().Be(timestamp);
            request.FromDate.Should().Be(fromDate);
            request.ToDate.Should().Be(toDate);
            request.Sports.Should().Equal(sports);
            request.Locations.Should().Equal(locations);
            request.Fixtures.Should().Equal(fixtures);
            request.Tournaments.Should().Equal(tournaments);
            request.Markets.Should().Equal(markets);
        }

        #endregion

        #region Edge Cases and Integration Tests

        [Fact]
        public void BaseRequest_CompleteInitialization_ShouldWork()
        {
            // Arrange & Act
            var request = new BaseRequest
            {
                PackageId = 12345,
                UserName = "testuser",
                Password = "testpass"
            };

            // Assert
            request.PackageId.Should().Be(12345);
            request.UserName.Should().Be("testuser");
            request.Password.Should().Be("testpass");
        }

        [Fact]
        public void GetMarketRequestDto_CompleteInitialization_ShouldWork()
        {
            // Arrange & Act
            var request = new GetMarketRequestDto
            {
                Timestamp = 1234567890L,
                FromDate = 1609459200L,
                ToDate = 1640995200L,
                Sports = new[] { 1, 2, 3 },
                Locations = new[] { 10, 20, 30 },
                Fixtures = new[] { 100, 200, 300 },
                Leagues = new[] { 1000, 2000, 3000 },
                Markets = new[] { 10000, 20000, 30000 }
            };

            // Assert
            request.Should().NotBeNull();
            request.Sports.Should().HaveCount(3);
            request.Locations.Should().HaveCount(3);
            request.Fixtures.Should().HaveCount(3);
            request.Leagues.Should().HaveCount(3);
            request.Markets.Should().HaveCount(3);
        }

        [Fact]
        public void GetOutrightMarketsRequestDto_CompleteInitialization_ShouldWork()
        {
            // Arrange & Act
            var request = new GetOutrightMarketsRequestDto
            {
                Timestamp = 1234567890L,
                FromDate = 1609459200L,
                ToDate = 1640995200L,
                Sports = new[] { 1, 2, 3 },
                Locations = new[] { 10, 20, 30 },
                Fixtures = new[] { 100, 200, 300 },
                Tournaments = new[] { 1000, 2000, 3000 },
                Markets = new[] { 10000, 20000, 30000 }
            };

            // Assert
            request.Should().NotBeNull();
            request.Sports.Should().HaveCount(3);
            request.Locations.Should().HaveCount(3);
            request.Fixtures.Should().HaveCount(3);
            request.Tournaments.Should().HaveCount(3);
            request.Markets.Should().HaveCount(3);
        }

        [Fact]
        public void RequestEntities_PropertyTypes_ShouldBeCorrect()
        {
            // Arrange
            var baseRequest = new BaseRequest();
            var marketRequest = new GetMarketRequestDto();
            var outrightRequest = new GetOutrightMarketsRequestDto();

            // Act & Assert - BaseRequest
            baseRequest.PackageId.Should().BeOfType(typeof(int));
            
            // Act & Assert - GetMarketRequestDto
            // Test nullable properties by setting them first
            marketRequest.Timestamp = 123L;
            marketRequest.Timestamp.Should().BeOfType(typeof(long));
            marketRequest.Sports.Should().BeAssignableTo<IEnumerable<int>>();
            
            // Act & Assert - GetOutrightMarketsRequestDto
            outrightRequest.Tournaments.Should().BeAssignableTo<IEnumerable<int>>();
        }

        #endregion
    }
} 