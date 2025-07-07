using Trade360SDK.Common.Entities.Enums;
using Xunit;
using FluentAssertions;
using System;
using System.Linq;

namespace Trade360SDK.Common.Tests.Entities.Enums
{
    public class BetStatusTests
    {
        [Fact]
        public void Enum_ShouldContainExpectedValues()
        {
            Assert.True(System.Enum.IsDefined(typeof(BetStatus), BetStatus.NotSet));
            Assert.True(System.Enum.IsDefined(typeof(BetStatus), BetStatus.Open));
            Assert.True(System.Enum.IsDefined(typeof(BetStatus), BetStatus.Suspended));
            Assert.True(System.Enum.IsDefined(typeof(BetStatus), BetStatus.Settled));
        }

        [Fact]
        public void BetStatus_ShouldHaveCorrectValues()
        {
            // Verify enum has expected numeric values
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
            int intValue = (int)status;

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
            BetStatus status = (BetStatus)intValue;

            // Assert
            status.Should().Be(expectedStatus);
        }

        [Theory]
        [InlineData("NotSet", BetStatus.NotSet)]
        [InlineData("Open", BetStatus.Open)]
        [InlineData("Suspended", BetStatus.Suspended)]
        [InlineData("Settled", BetStatus.Settled)]
        public void BetStatus_Parse_ShouldReturnCorrectEnum(string enumString, BetStatus expectedStatus)
        {
            // Act
            BetStatus status = Enum.Parse<BetStatus>(enumString);

            // Assert
            status.Should().Be(expectedStatus);
        }

        [Fact]
        public void BetStatus_ToString_ShouldReturnEnumName()
        {
            // Arrange & Act & Assert
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
        public void BetStatus_GetNames_ShouldReturnAllNames()
        {
            // Act
            var names = Enum.GetNames<BetStatus>();

            // Assert
            names.Should().HaveCount(4);
            names.Should().Contain("NotSet");
            names.Should().Contain("Open");
            names.Should().Contain("Suspended");
            names.Should().Contain("Settled");
        }

        [Theory]
        [InlineData(BetStatus.NotSet)]
        [InlineData(BetStatus.Open)]
        [InlineData(BetStatus.Suspended)]
        [InlineData(BetStatus.Settled)]
        public void BetStatus_IsDefined_ShouldReturnTrue(BetStatus status)
        {
            // Act & Assert
            Enum.IsDefined(typeof(BetStatus), status).Should().BeTrue();
        }

        [Theory]
        [InlineData(4)]
        [InlineData(-1)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void BetStatus_IsDefined_WithInvalidValues_ShouldReturnFalse(int invalidValue)
        {
            // Act & Assert
            Enum.IsDefined(typeof(BetStatus), invalidValue).Should().BeFalse();
        }

        [Fact]
        public void BetStatus_Type_ShouldBeEnum()
        {
            // Act & Assert
            typeof(BetStatus).IsEnum.Should().BeTrue();
            typeof(BetStatus).IsValueType.Should().BeTrue();
        }

        [Fact]
        public void BetStatus_UnderlyingType_ShouldBeInt32()
        {
            // Act
            var underlyingType = Enum.GetUnderlyingType(typeof(BetStatus));

            // Assert
            underlyingType.Should().Be(typeof(int));
        }

        [Fact]
        public void BetStatus_HasCorrectNamespace()
        {
            // Act & Assert
            typeof(BetStatus).Namespace.Should().Be("Trade360SDK.Common.Entities.Enums");
        }

        [Theory]
        [InlineData("notset")]
        [InlineData("OPEN")]
        [InlineData("suspended")]
        [InlineData("SETTLED")]
        public void BetStatus_Parse_WithIgnoreCase_ShouldWork(string enumString)
        {
            // Act & Assert - Should not throw
            var result = Enum.Parse<BetStatus>(enumString, ignoreCase: true);
            result.Should().BeOneOf(BetStatus.NotSet, BetStatus.Open, BetStatus.Suspended, BetStatus.Settled);
        }

        [Theory]
        [InlineData("")]
        [InlineData("InvalidStatus")]
        [InlineData(null)]
        public void BetStatus_Parse_WithInvalidString_ShouldThrow(string invalidString)
        {
            // Act & Assert
            if (invalidString == null)
            {
                Action act = () => Enum.Parse<BetStatus>(invalidString!);
                act.Should().Throw<ArgumentNullException>();
            }
            else
            {
                Action act = () => Enum.Parse<BetStatus>(invalidString);
                act.Should().Throw<ArgumentException>();
            }
        }

        [Fact]
        public void BetStatus_TryParse_WithValidString_ShouldReturnTrue()
        {
            // Act
            bool result = Enum.TryParse<BetStatus>("Open", out BetStatus status);

            // Assert
            result.Should().BeTrue();
            status.Should().Be(BetStatus.Open);
        }

        [Fact]
        public void BetStatus_TryParse_WithInvalidString_ShouldReturnFalse()
        {
            // Act
            bool result = Enum.TryParse<BetStatus>("InvalidStatus", out BetStatus status);

            // Assert
            result.Should().BeFalse();
            status.Should().Be(default(BetStatus)); // Should be NotSet (0)
        }

        [Theory]
        [InlineData("0", BetStatus.NotSet)]
        [InlineData("1", BetStatus.Open)]
        [InlineData("2", BetStatus.Suspended)]
        [InlineData("3", BetStatus.Settled)]
        public void BetStatus_Parse_WithNumericString_ShouldWork(string numericString, BetStatus expectedStatus)
        {
            // Act
            BetStatus result = Enum.Parse<BetStatus>(numericString);

            // Assert
            result.Should().Be(expectedStatus);
        }

        [Fact]
        public void BetStatus_Parse_WithUndefinedNumericString_ShouldCreateValue()
        {
            // Act - C# allows parsing undefined numeric values
            BetStatus result = Enum.Parse<BetStatus>("123");

            // Assert
            ((int)result).Should().Be(123);
            Enum.IsDefined(typeof(BetStatus), result).Should().BeFalse();
        }

        [Fact]
        public void BetStatus_Equality_ShouldWorkCorrectly()
        {
            // Arrange
            var status1 = BetStatus.Open;
            var status2 = BetStatus.Open;
            var status3 = BetStatus.Suspended;

            // Act & Assert
            (status1 == status2).Should().BeTrue();
            (status1 == status3).Should().BeFalse();
            (status1 != status3).Should().BeTrue();
            status1.Equals(status2).Should().BeTrue();
            status1.Equals(status3).Should().BeFalse();
        }

        [Fact]
        public void BetStatus_GetHashCode_ShouldBeConsistent()
        {
            // Arrange
            var status1 = BetStatus.Open;
            var status2 = BetStatus.Open;

            // Act
            var hash1 = status1.GetHashCode();
            var hash2 = status2.GetHashCode();

            // Assert
            hash1.Should().Be(hash2);
        }

        [Fact]
        public void BetStatus_CompareTo_ShouldWorkCorrectly()
        {
            // Act & Assert
            BetStatus.NotSet.CompareTo(BetStatus.Open).Should().BeLessThan(0);
            BetStatus.Open.CompareTo(BetStatus.Open).Should().Be(0);
            BetStatus.Settled.CompareTo(BetStatus.Suspended).Should().BeGreaterThan(0);
        }

        [Fact]
        public void BetStatus_DefaultValue_ShouldBeNotSet()
        {
            // Act
            var defaultStatus = default(BetStatus);

            // Assert
            defaultStatus.Should().Be(BetStatus.NotSet);
            ((int)defaultStatus).Should().Be(0);
        }

        [Fact]
        public void BetStatus_AllValues_ShouldHaveUniqueValues()
        {
            // Arrange
            var values = Enum.GetValues<BetStatus>();
            var intValues = values.Select(v => (int)v).ToArray();

            // Act & Assert
            intValues.Should().OnlyHaveUniqueItems();
            intValues.Length.Should().Be(values.Length);
        }

        [Fact]
        public void BetStatus_BusinessLogic_ValidationScenarios()
        {
            // Test business logic scenarios
            
            // NotSet should be the initial/default state
            var initialStatus = default(BetStatus);
            initialStatus.Should().Be(BetStatus.NotSet);
            
            // Open should indicate an active bet
            var activeBet = BetStatus.Open;
            activeBet.Should().NotBe(BetStatus.NotSet);
            activeBet.Should().NotBe(BetStatus.Settled);
            
            // Suspended should be different from both Open and Settled
            var suspendedBet = BetStatus.Suspended;
            suspendedBet.Should().NotBe(BetStatus.Open);
            suspendedBet.Should().NotBe(BetStatus.Settled);
            
            // Settled should be the final state
            var finalBet = BetStatus.Settled;
            finalBet.Should().NotBe(BetStatus.NotSet);
            finalBet.Should().NotBe(BetStatus.Open);
            finalBet.Should().NotBe(BetStatus.Suspended);
        }
    }
} 