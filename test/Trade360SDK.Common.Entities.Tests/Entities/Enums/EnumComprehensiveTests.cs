using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Tests.Entities.Enums
{
    public class FixtureStatusEnumTests
    {
        [Fact]
        public void FixtureStatus_ShouldHaveCorrectValues()
        {
            // Assert
            ((int)FixtureStatus.NotSet).Should().Be(0);
            ((int)FixtureStatus.NSY).Should().Be(1);
            ((int)FixtureStatus.InProgress).Should().Be(2);
            ((int)FixtureStatus.Finished).Should().Be(3);
            ((int)FixtureStatus.Cancelled).Should().Be(4);
            ((int)FixtureStatus.Postponed).Should().Be(5);
            ((int)FixtureStatus.Interrupted).Should().Be(6);
            ((int)FixtureStatus.Abandoned).Should().Be(7);
            ((int)FixtureStatus.LostCoverage).Should().Be(8);
            ((int)FixtureStatus.AboutToStart).Should().Be(9);
        }

        [Theory]
        [InlineData(FixtureStatus.NotSet, 0)]
        [InlineData(FixtureStatus.NSY, 1)]
        [InlineData(FixtureStatus.InProgress, 2)]
        [InlineData(FixtureStatus.Finished, 3)]
        [InlineData(FixtureStatus.Cancelled, 4)]
        [InlineData(FixtureStatus.Postponed, 5)]
        [InlineData(FixtureStatus.Interrupted, 6)]
        [InlineData(FixtureStatus.Abandoned, 7)]
        [InlineData(FixtureStatus.LostCoverage, 8)]
        [InlineData(FixtureStatus.AboutToStart, 9)]
        public void FixtureStatus_CastToInt_ShouldReturnCorrectValue(FixtureStatus status, int expectedValue)
        {
            // Act
            var intValue = (int)status;

            // Assert
            intValue.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(0, FixtureStatus.NotSet)]
        [InlineData(1, FixtureStatus.NSY)]
        [InlineData(2, FixtureStatus.InProgress)]
        [InlineData(3, FixtureStatus.Finished)]
        [InlineData(4, FixtureStatus.Cancelled)]
        [InlineData(5, FixtureStatus.Postponed)]
        [InlineData(6, FixtureStatus.Interrupted)]
        [InlineData(7, FixtureStatus.Abandoned)]
        [InlineData(8, FixtureStatus.LostCoverage)]
        [InlineData(9, FixtureStatus.AboutToStart)]
        public void FixtureStatus_CastFromInt_ShouldReturnCorrectEnum(int intValue, FixtureStatus expectedStatus)
        {
            // Act
            var status = (FixtureStatus)intValue;

            // Assert
            status.Should().Be(expectedStatus);
        }

        [Fact]
        public void FixtureStatus_ToString_ShouldReturnEnumName()
        {
            // Act & Assert
            FixtureStatus.NotSet.ToString().Should().Be("NotSet");
            FixtureStatus.NSY.ToString().Should().Be("NSY");
            FixtureStatus.InProgress.ToString().Should().Be("InProgress");
            FixtureStatus.Finished.ToString().Should().Be("Finished");
            FixtureStatus.Cancelled.ToString().Should().Be("Cancelled");
            FixtureStatus.Postponed.ToString().Should().Be("Postponed");
            FixtureStatus.Interrupted.ToString().Should().Be("Interrupted");
            FixtureStatus.Abandoned.ToString().Should().Be("Abandoned");
            FixtureStatus.LostCoverage.ToString().Should().Be("LostCoverage");
            FixtureStatus.AboutToStart.ToString().Should().Be("AboutToStart");
        }

        [Fact]
        public void FixtureStatus_GetValues_ShouldReturnAllValues()
        {
            // Act
            var values = Enum.GetValues<FixtureStatus>();

            // Assert
            values.Should().HaveCount(10);
            values.Should().Contain(FixtureStatus.NotSet);
            values.Should().Contain(FixtureStatus.NSY);
            values.Should().Contain(FixtureStatus.InProgress);
            values.Should().Contain(FixtureStatus.Finished);
            values.Should().Contain(FixtureStatus.Cancelled);
            values.Should().Contain(FixtureStatus.Postponed);
            values.Should().Contain(FixtureStatus.Interrupted);
            values.Should().Contain(FixtureStatus.Abandoned);
            values.Should().Contain(FixtureStatus.LostCoverage);
            values.Should().Contain(FixtureStatus.AboutToStart);
        }

        [Fact]
        public void FixtureStatus_IsDefined_ShouldReturnCorrectResult()
        {
            // Act & Assert
            Enum.IsDefined(typeof(FixtureStatus), 0).Should().BeTrue();
            Enum.IsDefined(typeof(FixtureStatus), 9).Should().BeTrue();
            Enum.IsDefined(typeof(FixtureStatus), 10).Should().BeFalse();
            Enum.IsDefined(typeof(FixtureStatus), -1).Should().BeFalse();
        }

        [Theory]
        [InlineData("NotSet", FixtureStatus.NotSet)]
        [InlineData("NSY", FixtureStatus.NSY)]
        [InlineData("InProgress", FixtureStatus.InProgress)]
        [InlineData("Finished", FixtureStatus.Finished)]
        [InlineData("Cancelled", FixtureStatus.Cancelled)]
        [InlineData("Postponed", FixtureStatus.Postponed)]
        [InlineData("Interrupted", FixtureStatus.Interrupted)]
        [InlineData("Abandoned", FixtureStatus.Abandoned)]
        [InlineData("LostCoverage", FixtureStatus.LostCoverage)]
        [InlineData("AboutToStart", FixtureStatus.AboutToStart)]
        public void FixtureStatus_Parse_ShouldReturnCorrectEnum(string enumString, FixtureStatus expectedStatus)
        {
            // Act
            var status = Enum.Parse<FixtureStatus>(enumString);

            // Assert
            status.Should().Be(expectedStatus);
        }
    }

    public class BetStatusEnumTests
    {
        [Fact]
        public void BetStatus_ShouldHaveCorrectValues()
        {
            // Assert
            ((int)BetStatus.NotSet).Should().Be(0);
            ((int)BetStatus.Open).Should().Be(1);
            ((int)BetStatus.Suspended).Should().Be(2);
            ((int)BetStatus.Settled).Should().Be(3);
        }

        [Theory]
        [InlineData(BetStatus.NotSet, 0)]
        [InlineData(BetStatus.Open, 1)]
        [InlineData(BetStatus.Suspended, 2)]
        [InlineData(BetStatus.Settled, 3)]
        public void BetStatus_CastToInt_ShouldReturnCorrectValue(BetStatus status, int expectedValue)
        {
            // Act
            var intValue = (int)status;

            // Assert
            intValue.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(0, BetStatus.NotSet)]
        [InlineData(1, BetStatus.Open)]
        [InlineData(2, BetStatus.Suspended)]
        [InlineData(3, BetStatus.Settled)]
        public void BetStatus_CastFromInt_ShouldReturnCorrectEnum(int intValue, BetStatus expectedStatus)
        {
            // Act
            var status = (BetStatus)intValue;

            // Assert
            status.Should().Be(expectedStatus);
        }

        [Fact]
        public void BetStatus_ToString_ShouldReturnEnumName()
        {
            // Act & Assert
            BetStatus.NotSet.ToString().Should().Be("NotSet");
            BetStatus.Open.ToString().Should().Be("Open");
            BetStatus.Suspended.ToString().Should().Be("Suspended");
            BetStatus.Settled.ToString().Should().Be("Settled");
        }

        [Fact]
        public void BetStatus_GetValues_ShouldReturnAllValues()
        {
            // Act
            var values = Enum.GetValues<BetStatus>();

            // Assert
            values.Should().HaveCount(4);
            values.Should().Contain(BetStatus.NotSet);
            values.Should().Contain(BetStatus.Open);
            values.Should().Contain(BetStatus.Suspended);
            values.Should().Contain(BetStatus.Settled);
        }

        [Fact]
        public void BetStatus_IsDefined_ShouldReturnCorrectResult()
        {
            // Act & Assert
            Enum.IsDefined(typeof(BetStatus), 0).Should().BeTrue();
            Enum.IsDefined(typeof(BetStatus), 3).Should().BeTrue();
            Enum.IsDefined(typeof(BetStatus), 4).Should().BeFalse();
            Enum.IsDefined(typeof(BetStatus), -1).Should().BeFalse();
        }

        [Theory]
        [InlineData("NotSet", BetStatus.NotSet)]
        [InlineData("Open", BetStatus.Open)]
        [InlineData("Suspended", BetStatus.Suspended)]
        [InlineData("Settled", BetStatus.Settled)]
        public void BetStatus_Parse_ShouldReturnCorrectEnum(string enumString, BetStatus expectedStatus)
        {
            // Act
            var status = Enum.Parse<BetStatus>(enumString);

            // Assert
            status.Should().Be(expectedStatus);
        }
    }

    public class SubscriptionStateEnumTests
    {
        [Fact]
        public void SubscriptionState_ShouldHaveCorrectValues()
        {
            // Assert
            ((int)SubscriptionState.All).Should().Be(0);
            ((int)SubscriptionState.NotSubscribed).Should().Be(1);
            ((int)SubscriptionState.Subscribed).Should().Be(2);
        }

        [Theory]
        [InlineData(SubscriptionState.All, 0)]
        [InlineData(SubscriptionState.NotSubscribed, 1)]
        [InlineData(SubscriptionState.Subscribed, 2)]
        public void SubscriptionState_CastToInt_ShouldReturnCorrectValue(SubscriptionState state, int expectedValue)
        {
            // Act
            var intValue = (int)state;

            // Assert
            intValue.Should().Be(expectedValue);
        }

        [Fact]
        public void SubscriptionState_ToString_ShouldReturnEnumName()
        {
            // Act & Assert
            SubscriptionState.All.ToString().Should().Be("All");
            SubscriptionState.NotSubscribed.ToString().Should().Be("NotSubscribed");
            SubscriptionState.Subscribed.ToString().Should().Be("Subscribed");
        }

        [Fact]
        public void SubscriptionState_GetValues_ShouldReturnAllValues()
        {
            // Act
            var values = Enum.GetValues<SubscriptionState>();

            // Assert
            values.Should().HaveCount(3);
            values.Should().Contain(SubscriptionState.All);
            values.Should().Contain(SubscriptionState.NotSubscribed);
            values.Should().Contain(SubscriptionState.Subscribed);
        }
    }

    public class DangerIndicatorStatusEnumTests
    {
        [Fact]
        public void DangerIndicatorStatus_ShouldHaveCorrectValues()
        {
            // Assert
            ((int)DangerIndicatorStatus.Safe).Should().Be(1);
            ((int)DangerIndicatorStatus.Danger).Should().Be(2);
        }

        [Theory]
        [InlineData(DangerIndicatorStatus.Safe, 1)]
        [InlineData(DangerIndicatorStatus.Danger, 2)]
        public void DangerIndicatorStatus_CastToInt_ShouldReturnCorrectValue(DangerIndicatorStatus status, int expectedValue)
        {
            // Act
            var intValue = (int)status;

            // Assert
            intValue.Should().Be(expectedValue);
        }

        [Fact]
        public void DangerIndicatorStatus_ToString_ShouldReturnEnumName()
        {
            // Act & Assert
            DangerIndicatorStatus.Safe.ToString().Should().Be("Safe");
            DangerIndicatorStatus.Danger.ToString().Should().Be("Danger");
        }
    }

    public class DangerIndicatorTypeEnumTests
    {
        [Fact]
        public void DangerIndicatorType_ShouldHaveCorrectValues()
        {
            // Assert
            ((int)DangerIndicatorType.General).Should().Be(1);
            ((int)DangerIndicatorType.Cards).Should().Be(2);
            ((int)DangerIndicatorType.Corners).Should().Be(3);
        }

        [Theory]
        [InlineData(DangerIndicatorType.General, 1)]
        [InlineData(DangerIndicatorType.Cards, 2)]
        [InlineData(DangerIndicatorType.Corners, 3)]
        public void DangerIndicatorType_CastToInt_ShouldReturnCorrectValue(DangerIndicatorType type, int expectedValue)
        {
            // Act
            var intValue = (int)type;

            // Assert
            intValue.Should().Be(expectedValue);
        }

        [Fact]
        public void DangerIndicatorType_ToString_ShouldReturnEnumName()
        {
            // Act & Assert
            DangerIndicatorType.General.ToString().Should().Be("General");
            DangerIndicatorType.Cards.ToString().Should().Be("Cards");
            DangerIndicatorType.Corners.ToString().Should().Be("Corners");
        }
    }

    public class EnumGeneralTests
    {
        [Fact]
        public void SomeEnums_ShouldHaveNotSetAsZero()
        {
            // Assert - Common pattern in the codebase for some enums
            ((int)FixtureStatus.NotSet).Should().Be(0);
            ((int)BetStatus.NotSet).Should().Be(0);
        }

        [Fact]
        public void SubscriptionState_ShouldHaveAllAsZero()
        {
            // Assert - SubscriptionState uses All as the default/zero value
            ((int)SubscriptionState.All).Should().Be(0);
        }

        [Fact]
        public void AllEnums_ShouldBeValueTypes()
        {
            // Assert
            typeof(FixtureStatus).IsValueType.Should().BeTrue();
            typeof(BetStatus).IsValueType.Should().BeTrue();
            typeof(SubscriptionState).IsValueType.Should().BeTrue();
            typeof(DangerIndicatorStatus).IsValueType.Should().BeTrue();
            typeof(DangerIndicatorType).IsValueType.Should().BeTrue();
        }

        [Fact]
        public void AllEnums_ShouldBeEnumType()
        {
            // Assert
            typeof(FixtureStatus).IsEnum.Should().BeTrue();
            typeof(BetStatus).IsEnum.Should().BeTrue();
            typeof(SubscriptionState).IsEnum.Should().BeTrue();
            typeof(DangerIndicatorStatus).IsEnum.Should().BeTrue();
            typeof(DangerIndicatorType).IsEnum.Should().BeTrue();
        }

        [Fact]
        public void AllEnums_ShouldHaveCorrectNamespace()
        {
            // Assert
            typeof(FixtureStatus).Namespace.Should().Be("Trade360SDK.Common.Entities.Enums");
            typeof(BetStatus).Namespace.Should().Be("Trade360SDK.Common.Entities.Enums");
            typeof(SubscriptionState).Namespace.Should().Be("Trade360SDK.Common.Entities.Enums");
            typeof(DangerIndicatorStatus).Namespace.Should().Be("Trade360SDK.Common.Entities.Enums");
            typeof(DangerIndicatorType).Namespace.Should().Be("Trade360SDK.Common.Entities.Enums");
        }
    }
} 