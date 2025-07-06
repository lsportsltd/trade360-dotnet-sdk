using System;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Entities.Livescore;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Tests.Entities.Livescore
{
    public class DangerIndicatorComprehensiveTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var dangerIndicator = new DangerIndicator();

            // Assert
            dangerIndicator.Type.Should().Be(default(DangerIndicatorType));
            dangerIndicator.Status.Should().Be(default(DangerIndicatorStatus));
            dangerIndicator.LastUpdate.Should().Be(default(DateTime));
        }

        [Theory]
        [InlineData(DangerIndicatorType.General)]
        [InlineData(DangerIndicatorType.Cards)]
        [InlineData(DangerIndicatorType.Corners)]
        public void Type_WithVariousValues_ShouldStoreCorrectly(DangerIndicatorType type)
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();

            // Act
            dangerIndicator.Type = type;

            // Assert
            dangerIndicator.Type.Should().Be(type);
        }

        [Theory]
        [InlineData(DangerIndicatorStatus.Safe)]
        [InlineData(DangerIndicatorStatus.Danger)]
        public void Status_WithVariousValues_ShouldStoreCorrectly(DangerIndicatorStatus status)
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();

            // Act
            dangerIndicator.Status = status;

            // Assert
            dangerIndicator.Status.Should().Be(status);
        }

        [Fact]
        public void LastUpdate_WithCurrentDateTime_ShouldStoreCorrectly()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();
            var currentTime = DateTime.Now;

            // Act
            dangerIndicator.LastUpdate = currentTime;

            // Assert
            dangerIndicator.LastUpdate.Should().Be(currentTime);
        }

        [Fact]
        public void LastUpdate_WithUtcDateTime_ShouldStoreCorrectly()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();
            var utcTime = DateTime.UtcNow;

            // Act
            dangerIndicator.LastUpdate = utcTime;

            // Assert
            dangerIndicator.LastUpdate.Should().Be(utcTime);
        }

        [Theory]
        [InlineData("2023-01-01")]
        [InlineData("2025-12-31")]
        [InlineData("1900-01-01")]
        public void LastUpdate_WithSpecificDates_ShouldStoreCorrectly(string dateString)
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();
            var specificDate = DateTime.Parse(dateString);

            // Act
            dangerIndicator.LastUpdate = specificDate;

            // Assert
            dangerIndicator.LastUpdate.Should().Be(specificDate);
        }

        [Fact]
        public void Properties_CanBeSetIndependently()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();
            var expectedType = DangerIndicatorType.Cards;
            var expectedStatus = DangerIndicatorStatus.Danger;
            var expectedLastUpdate = new DateTime(2023, 6, 15, 14, 30, 0);

            // Act
            dangerIndicator.Type = expectedType;
            dangerIndicator.Status = expectedStatus;
            dangerIndicator.LastUpdate = expectedLastUpdate;

            // Assert
            dangerIndicator.Type.Should().Be(expectedType);
            dangerIndicator.Status.Should().Be(expectedStatus);
            dangerIndicator.LastUpdate.Should().Be(expectedLastUpdate);
        }

        [Fact]
        public void Properties_CanBeOverwritten()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator
            {
                Type = DangerIndicatorType.General,
                Status = DangerIndicatorStatus.Safe,
                LastUpdate = DateTime.MinValue
            };

            var newType = DangerIndicatorType.Corners;
            var newStatus = DangerIndicatorStatus.Danger;
            var newLastUpdate = DateTime.MaxValue;

            // Act
            dangerIndicator.Type = newType;
            dangerIndicator.Status = newStatus;
            dangerIndicator.LastUpdate = newLastUpdate;

            // Assert
            dangerIndicator.Type.Should().Be(newType);
            dangerIndicator.Status.Should().Be(newStatus);
            dangerIndicator.LastUpdate.Should().Be(newLastUpdate);
        }

        [Fact]
        public void ObjectInitializer_ShouldSetAllProperties()
        {
            // Arrange
            var expectedType = DangerIndicatorType.Cards;
            var expectedStatus = DangerIndicatorStatus.Danger;
            var expectedLastUpdate = new DateTime(2023, 12, 25, 18, 45, 30);

            // Act
            var dangerIndicator = new DangerIndicator
            {
                Type = expectedType,
                Status = expectedStatus,
                LastUpdate = expectedLastUpdate
            };

            // Assert
            dangerIndicator.Type.Should().Be(expectedType);
            dangerIndicator.Status.Should().Be(expectedStatus);
            dangerIndicator.LastUpdate.Should().Be(expectedLastUpdate);
        }

        [Fact]
        public void LastUpdate_WithMinValue_ShouldStoreCorrectly()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();

            // Act
            dangerIndicator.LastUpdate = DateTime.MinValue;

            // Assert
            dangerIndicator.LastUpdate.Should().Be(DateTime.MinValue);
        }

        [Fact]
        public void LastUpdate_WithMaxValue_ShouldStoreCorrectly()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();

            // Act
            dangerIndicator.LastUpdate = DateTime.MaxValue;

            // Assert
            dangerIndicator.LastUpdate.Should().Be(DateTime.MaxValue);
        }

        [Fact]
        public void DangerIndicator_ShouldSupportPropertyChaining()
        {
            // Arrange & Act
            var dangerIndicator = new DangerIndicator();
            dangerIndicator.Type = DangerIndicatorType.General;
            dangerIndicator.Status = DangerIndicatorStatus.Safe;
            dangerIndicator.LastUpdate = DateTime.Today;

            // Assert
            dangerIndicator.Type.Should().Be(DangerIndicatorType.General);
            dangerIndicator.Status.Should().Be(DangerIndicatorStatus.Safe);
            dangerIndicator.LastUpdate.Should().Be(DateTime.Today);
        }

        [Fact]
        public void ToString_ShouldNotThrow()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator
            {
                Type = DangerIndicatorType.Cards,
                Status = DangerIndicatorStatus.Danger,
                LastUpdate = DateTime.Now
            };

            // Act & Assert
            var act = () => dangerIndicator.ToString();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetHashCode_ShouldNotThrow()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator
            {
                Type = DangerIndicatorType.Corners,
                Status = DangerIndicatorStatus.Safe,
                LastUpdate = DateTime.UtcNow
            };

            // Act & Assert
            var act = () => dangerIndicator.GetHashCode();
            act.Should().NotThrow();
        }

        [Fact]
        public void DangerIndicator_WithGeneralTypeSafeStatus_ShouldRepresentNormalCondition()
        {
            // Arrange & Act
            var dangerIndicator = new DangerIndicator
            {
                Type = DangerIndicatorType.General,
                Status = DangerIndicatorStatus.Safe,
                LastUpdate = DateTime.UtcNow
            };

            // Assert
            dangerIndicator.Type.Should().Be(DangerIndicatorType.General);
            dangerIndicator.Status.Should().Be(DangerIndicatorStatus.Safe);
            dangerIndicator.LastUpdate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void DangerIndicator_WithCardsTypeDangerStatus_ShouldRepresentCardsDanger()
        {
            // Arrange & Act
            var dangerIndicator = new DangerIndicator
            {
                Type = DangerIndicatorType.Cards,
                Status = DangerIndicatorStatus.Danger,
                LastUpdate = DateTime.UtcNow
            };

            // Assert
            dangerIndicator.Type.Should().Be(DangerIndicatorType.Cards);
            dangerIndicator.Status.Should().Be(DangerIndicatorStatus.Danger);
            dangerIndicator.LastUpdate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void DangerIndicator_WithCornersTypeDangerStatus_ShouldRepresentCornersDanger()
        {
            // Arrange & Act
            var dangerIndicator = new DangerIndicator
            {
                Type = DangerIndicatorType.Corners,
                Status = DangerIndicatorStatus.Danger,
                LastUpdate = DateTime.UtcNow
            };

            // Assert
            dangerIndicator.Type.Should().Be(DangerIndicatorType.Corners);
            dangerIndicator.Status.Should().Be(DangerIndicatorStatus.Danger);
            dangerIndicator.LastUpdate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Theory]
        [InlineData(DangerIndicatorType.General, DangerIndicatorStatus.Safe)]
        [InlineData(DangerIndicatorType.General, DangerIndicatorStatus.Danger)]
        [InlineData(DangerIndicatorType.Cards, DangerIndicatorStatus.Safe)]
        [InlineData(DangerIndicatorType.Cards, DangerIndicatorStatus.Danger)]
        [InlineData(DangerIndicatorType.Corners, DangerIndicatorStatus.Safe)]
        [InlineData(DangerIndicatorType.Corners, DangerIndicatorStatus.Danger)]
        public void DangerIndicator_WithVariousCombinations_ShouldSetCorrectly(DangerIndicatorType type, DangerIndicatorStatus status)
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();
            var lastUpdate = DateTime.UtcNow;

            // Act
            dangerIndicator.Type = type;
            dangerIndicator.Status = status;
            dangerIndicator.LastUpdate = lastUpdate;

            // Assert
            dangerIndicator.Type.Should().Be(type);
            dangerIndicator.Status.Should().Be(status);
            dangerIndicator.LastUpdate.Should().Be(lastUpdate);
        }

        [Fact]
        public void DangerIndicator_WithRecentUpdate_ShouldIndicateCurrentStatus()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();
            var recentTime = DateTime.UtcNow.AddMinutes(-1);

            // Act
            dangerIndicator.Type = DangerIndicatorType.General;
            dangerIndicator.Status = DangerIndicatorStatus.Danger;
            dangerIndicator.LastUpdate = recentTime;

            // Assert
            dangerIndicator.Type.Should().Be(DangerIndicatorType.General);
            dangerIndicator.Status.Should().Be(DangerIndicatorStatus.Danger);
            dangerIndicator.LastUpdate.Should().Be(recentTime);
            dangerIndicator.LastUpdate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(2));
        }

        [Fact]
        public void DangerIndicator_WithOldUpdate_ShouldStillMaintainData()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();
            var oldTime = DateTime.UtcNow.AddHours(-24);

            // Act
            dangerIndicator.Type = DangerIndicatorType.Corners;
            dangerIndicator.Status = DangerIndicatorStatus.Safe;
            dangerIndicator.LastUpdate = oldTime;

            // Assert
            dangerIndicator.Type.Should().Be(DangerIndicatorType.Corners);
            dangerIndicator.Status.Should().Be(DangerIndicatorStatus.Safe);
            dangerIndicator.LastUpdate.Should().Be(oldTime);
            dangerIndicator.LastUpdate.Should().BeBefore(DateTime.UtcNow.AddHours(-23));
        }
    }
} 