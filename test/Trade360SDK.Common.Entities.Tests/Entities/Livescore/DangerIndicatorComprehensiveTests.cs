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
        public void DangerIndicator_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Act
            var dangerIndicator = new DangerIndicator();

            // Assert
            dangerIndicator.Should().NotBeNull();
            dangerIndicator.Type.Should().Be((DangerIndicatorType)0);
            dangerIndicator.Status.Should().Be((DangerIndicatorStatus)0);
            dangerIndicator.LastUpdate.Should().Be(default(DateTime));
        }

        [Fact]
        public void DangerIndicator_SetType_ShouldSetValue()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();
            var type = DangerIndicatorType.Cards;

            // Act
            dangerIndicator.Type = type;

            // Assert
            dangerIndicator.Type.Should().Be(type);
        }

        [Theory]
        [InlineData(DangerIndicatorType.General)]
        [InlineData(DangerIndicatorType.Cards)]
        [InlineData(DangerIndicatorType.Corners)]
        public void DangerIndicator_SetVariousTypes_ShouldSetValue(DangerIndicatorType type)
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();

            // Act
            dangerIndicator.Type = type;

            // Assert
            dangerIndicator.Type.Should().Be(type);
        }

        [Fact]
        public void DangerIndicator_SetStatus_ShouldSetValue()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();
            var status = DangerIndicatorStatus.Danger;

            // Act
            dangerIndicator.Status = status;

            // Assert
            dangerIndicator.Status.Should().Be(status);
        }

        [Theory]
        [InlineData(DangerIndicatorStatus.Safe)]
        [InlineData(DangerIndicatorStatus.Danger)]
        public void DangerIndicator_SetVariousStatuses_ShouldSetValue(DangerIndicatorStatus status)
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();

            // Act
            dangerIndicator.Status = status;

            // Assert
            dangerIndicator.Status.Should().Be(status);
        }

        [Fact]
        public void DangerIndicator_SetLastUpdate_ShouldSetValue()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();
            var lastUpdate = DateTime.UtcNow;

            // Act
            dangerIndicator.LastUpdate = lastUpdate;

            // Assert
            dangerIndicator.LastUpdate.Should().Be(lastUpdate);
        }

        [Fact]
        public void DangerIndicator_SetLastUpdateWithSpecificDate_ShouldSetValue()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();
            var lastUpdate = new DateTime(2023, 12, 25, 15, 30, 45, DateTimeKind.Utc);

            // Act
            dangerIndicator.LastUpdate = lastUpdate;

            // Assert
            dangerIndicator.LastUpdate.Should().Be(lastUpdate);
            dangerIndicator.LastUpdate.Year.Should().Be(2023);
            dangerIndicator.LastUpdate.Month.Should().Be(12);
            dangerIndicator.LastUpdate.Day.Should().Be(25);
            dangerIndicator.LastUpdate.Hour.Should().Be(15);
            dangerIndicator.LastUpdate.Minute.Should().Be(30);
            dangerIndicator.LastUpdate.Second.Should().Be(45);
        }

        [Fact]
        public void DangerIndicator_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();
            var type = DangerIndicatorType.Corners;
            var status = DangerIndicatorStatus.Danger;
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
        public void DangerIndicator_WithMinDateTime_ShouldSetValue()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();
            var minDateTime = DateTime.MinValue;

            // Act
            dangerIndicator.LastUpdate = minDateTime;

            // Assert
            dangerIndicator.LastUpdate.Should().Be(minDateTime);
        }

        [Fact]
        public void DangerIndicator_WithMaxDateTime_ShouldSetValue()
        {
            // Arrange
            var dangerIndicator = new DangerIndicator();
            var maxDateTime = DateTime.MaxValue;

            // Act
            dangerIndicator.LastUpdate = maxDateTime;

            // Assert
            dangerIndicator.LastUpdate.Should().Be(maxDateTime);
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