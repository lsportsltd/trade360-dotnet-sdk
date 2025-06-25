using System;
using FluentAssertions;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.Markets
{
    public class ProviderBetComprehensiveTests
    {
        #region Constructor Tests

        [Fact]
        public void ProviderBet_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var providerBet = new ProviderBet();

            // Assert
            providerBet.Should().NotBeNull();
            providerBet.Should().BeAssignableTo<BaseBet>();
            
            // Check inherited properties are initialized to default values
            providerBet.Id.Should().Be(0);
            providerBet.Name.Should().BeNull();
            providerBet.Status.Should().Be(default(BetStatus));
            providerBet.Price.Should().BeNull();
            providerBet.PriceUS.Should().BeNull();
            providerBet.PriceUK.Should().BeNull();
            providerBet.StartPrice.Should().BeNull();
            providerBet.PriceIN.Should().BeNull();
            providerBet.PriceMA.Should().BeNull();
            providerBet.PriceHK.Should().BeNull();
            providerBet.PriceVolume.Should().BeNull();
            providerBet.Probability.Should().Be(0.0);
            providerBet.LastUpdate.Should().Be(default(DateTime));
        }

        #endregion

        #region Inheritance Tests

        [Fact]
        public void ProviderBet_ShouldInheritFromBaseBet()
        {
            // Act
            var providerBet = new ProviderBet();

            // Assert
            providerBet.Should().BeAssignableTo<BaseBet>();
        }

        [Fact]
        public void ProviderBet_ShouldHaveAllBaseBetProperties()
        {
            // Arrange
            var providerBet = new ProviderBet();

            // Act & Assert - Test inherited properties can be set
            providerBet.Id = 123;
            providerBet.Name = "Test Bet";
                         providerBet.Status = BetStatus.Open;
            providerBet.Price = "1.85";
            providerBet.PriceUS = "+185";
            providerBet.PriceUK = "17/20";
            providerBet.StartPrice = "1.90";
            providerBet.PriceIN = "0.85";
            providerBet.PriceMA = "0.85";
            providerBet.PriceHK = "0.85";
            providerBet.PriceVolume = "1000";
            providerBet.Probability = 0.54;
            providerBet.LastUpdate = DateTime.Now;

                         // Verify all properties are set correctly
             providerBet.Id.Should().Be(123);
             providerBet.Name.Should().Be("Test Bet");
             providerBet.Status.Should().Be(BetStatus.Open);
            providerBet.Price.Should().Be("1.85");
            providerBet.PriceUS.Should().Be("+185");
            providerBet.PriceUK.Should().Be("17/20");
            providerBet.StartPrice.Should().Be("1.90");
            providerBet.PriceIN.Should().Be("0.85");
            providerBet.PriceMA.Should().Be("0.85");
            providerBet.PriceHK.Should().Be("0.85");
            providerBet.PriceVolume.Should().Be("1000");
            providerBet.Probability.Should().Be(0.54);
            providerBet.LastUpdate.Should().NotBe(default(DateTime));
        }

        #endregion

        #region Property Tests via Inheritance

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999999)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void ProviderBet_IdProperty_ShouldSetAndGetCorrectly(int id)
        {
            // Arrange
            var providerBet = new ProviderBet();

            // Act
            providerBet.Id = id;

            // Assert
            providerBet.Id.Should().Be(id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Home Win")]
        [InlineData("Draw")]
        [InlineData("Away Win")]
        [InlineData("Over 2.5")]
        [InlineData("Under 2.5")]
        [InlineData("Very Long Bet Name That Might Be Used In Real World")]
        [InlineData("Bet123")]
        [InlineData("Bet-With-Dashes")]
        [InlineData("Bet_With_Underscores")]
        [InlineData("Bet.With.Dots")]
        [InlineData("Bet With Spaces")]
        public void ProviderBet_NameProperty_ShouldSetAndGetCorrectly(string? name)
        {
            // Arrange
            var providerBet = new ProviderBet();

            // Act
            providerBet.Name = name;

            // Assert
            providerBet.Name.Should().Be(name);
        }

        [Theory]
        [InlineData(BetStatus.NotSet)]
        [InlineData(BetStatus.Open)]
        [InlineData(BetStatus.Suspended)]
        [InlineData(BetStatus.Settled)]
        public void ProviderBet_StatusProperty_ShouldSetAndGetCorrectly(BetStatus status)
        {
            // Arrange
            var providerBet = new ProviderBet();

            // Act
            providerBet.Status = status;

            // Assert
            providerBet.Status.Should().Be(status);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1.50")]
        [InlineData("2.00")]
        [InlineData("10.50")]
        [InlineData("999.99")]
        [InlineData("1.01")]
        public void ProviderBet_PriceProperty_ShouldSetAndGetCorrectly(string? price)
        {
            // Arrange
            var providerBet = new ProviderBet();

            // Act
            providerBet.Price = price;

            // Assert
            providerBet.Price.Should().Be(price);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(0.1)]
        [InlineData(0.5)]
        [InlineData(0.9)]
        [InlineData(1.0)]
        [InlineData(50.0)]
        [InlineData(100.0)]
        public void ProviderBet_ProbabilityProperty_ShouldSetAndGetCorrectly(double probability)
        {
            // Arrange
            var providerBet = new ProviderBet();

            // Act
            providerBet.Probability = probability;

            // Assert
            providerBet.Probability.Should().Be(probability);
        }

        [Fact]
        public void ProviderBet_LastUpdateProperty_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var providerBet = new ProviderBet();
            var testDate = new DateTime(2024, 6, 25, 14, 30, 45);

            // Act
            providerBet.LastUpdate = testDate;

            // Assert
            providerBet.LastUpdate.Should().Be(testDate);
        }

        #endregion

        #region Edge Case Tests

        [Fact]
        public void ProviderBet_WithMinDateTime_ShouldHandleCorrectly()
        {
            // Arrange
            var providerBet = new ProviderBet();

            // Act
            providerBet.LastUpdate = DateTime.MinValue;

            // Assert
            providerBet.LastUpdate.Should().Be(DateTime.MinValue);
        }

        [Fact]
        public void ProviderBet_WithMaxDateTime_ShouldHandleCorrectly()
        {
            // Arrange
            var providerBet = new ProviderBet();

            // Act
            providerBet.LastUpdate = DateTime.MaxValue;

            // Assert
            providerBet.LastUpdate.Should().Be(DateTime.MaxValue);
        }

        [Fact]
        public void ProviderBet_WithNaNProbability_ShouldHandleCorrectly()
        {
            // Arrange
            var providerBet = new ProviderBet();

            // Act
            providerBet.Probability = double.NaN;

            // Assert
            providerBet.Probability.Should().Be(double.NaN);
        }

        [Fact]
        public void ProviderBet_WithPositiveInfinityProbability_ShouldHandleCorrectly()
        {
            // Arrange
            var providerBet = new ProviderBet();

            // Act
            providerBet.Probability = double.PositiveInfinity;

            // Assert
            providerBet.Probability.Should().Be(double.PositiveInfinity);
        }

        [Fact]
        public void ProviderBet_WithNegativeInfinityProbability_ShouldHandleCorrectly()
        {
            // Arrange
            var providerBet = new ProviderBet();

            // Act
            providerBet.Probability = double.NegativeInfinity;

            // Assert
            providerBet.Probability.Should().Be(double.NegativeInfinity);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\r\n")]
        public void ProviderBet_WithWhitespaceStrings_ShouldPreserveWhitespace(string whitespace)
        {
            // Arrange
            var providerBet = new ProviderBet();

            // Act
            providerBet.Name = whitespace;
            providerBet.Price = whitespace;
            providerBet.PriceUS = whitespace;
            providerBet.PriceUK = whitespace;

            // Assert
            providerBet.Name.Should().Be(whitespace);
            providerBet.Price.Should().Be(whitespace);
            providerBet.PriceUS.Should().Be(whitespace);
            providerBet.PriceUK.Should().Be(whitespace);
        }

        [Fact]
        public void ProviderBet_WithUnicodeStrings_ShouldHandleCorrectly()
        {
            // Arrange
            var providerBet = new ProviderBet();
            var unicodeName = "Bet™ 中文 🎯 ñáéíóú";
            var unicodePrice = "€1.85";

            // Act
            providerBet.Name = unicodeName;
            providerBet.Price = unicodePrice;

            // Assert
            providerBet.Name.Should().Be(unicodeName);
            providerBet.Price.Should().Be(unicodePrice);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void ProviderBet_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var providerBet = new ProviderBet();
            var id = 999;
            var name = "Test Bet";
                         var status = BetStatus.Open;
            var price = "2.50";
            var priceUS = "+150";
            var priceUK = "3/2";
            var startPrice = "2.40";
            var priceIN = "1.50";
            var priceMA = "1.50";
            var priceHK = "1.50";
            var priceVolume = "5000";
            var probability = 0.40;
            var lastUpdate = new DateTime(2024, 6, 25, 14, 30, 45);

            // Act
            providerBet.Id = id;
            providerBet.Name = name;
            providerBet.Status = status;
            providerBet.Price = price;
            providerBet.PriceUS = priceUS;
            providerBet.PriceUK = priceUK;
            providerBet.StartPrice = startPrice;
            providerBet.PriceIN = priceIN;
            providerBet.PriceMA = priceMA;
            providerBet.PriceHK = priceHK;
            providerBet.PriceVolume = priceVolume;
            providerBet.Probability = probability;
            providerBet.LastUpdate = lastUpdate;

            // Assert
            providerBet.Id.Should().Be(id);
            providerBet.Name.Should().Be(name);
            providerBet.Status.Should().Be(status);
            providerBet.Price.Should().Be(price);
            providerBet.PriceUS.Should().Be(priceUS);
            providerBet.PriceUK.Should().Be(priceUK);
            providerBet.StartPrice.Should().Be(startPrice);
            providerBet.PriceIN.Should().Be(priceIN);
            providerBet.PriceMA.Should().Be(priceMA);
            providerBet.PriceHK.Should().Be(priceHK);
            providerBet.PriceVolume.Should().Be(priceVolume);
            providerBet.Probability.Should().Be(probability);
            providerBet.LastUpdate.Should().Be(lastUpdate);
        }

        [Fact]
        public void ProviderBet_SetAllPropertiesToNull_ShouldSetNullValues()
        {
            // Arrange
                         var providerBet = new ProviderBet
             {
                 Id = 123,
                 Name = "Initial Name",
                 Status = BetStatus.Open,
                Price = "1.50",
                PriceUS = "+150",
                PriceUK = "1/2",
                StartPrice = "1.60",
                PriceIN = "0.50",
                PriceMA = "0.50",
                PriceHK = "0.50",
                PriceVolume = "1000",
                Probability = 0.67,
                LastUpdate = DateTime.Now
            };

            // Act
            providerBet.Name = null;
            providerBet.Price = null;
            providerBet.PriceUS = null;
            providerBet.PriceUK = null;
            providerBet.StartPrice = null;
            providerBet.PriceIN = null;
            providerBet.PriceMA = null;
            providerBet.PriceHK = null;
            providerBet.PriceVolume = null;

            // Assert
            providerBet.Id.Should().Be(123); // Value type, can't be null
                         providerBet.Status.Should().Be(BetStatus.Open); // Enum, can't be null
            providerBet.Probability.Should().Be(0.67); // Value type, can't be null
            providerBet.Name.Should().BeNull();
            providerBet.Price.Should().BeNull();
            providerBet.PriceUS.Should().BeNull();
            providerBet.PriceUK.Should().BeNull();
            providerBet.StartPrice.Should().BeNull();
            providerBet.PriceIN.Should().BeNull();
            providerBet.PriceMA.Should().BeNull();
            providerBet.PriceHK.Should().BeNull();
            providerBet.PriceVolume.Should().BeNull();
        }

        #endregion

        #region Behavior Tests

        [Fact]
        public void ProviderBet_MultipleAssignments_ShouldOverwritePreviousValues()
        {
            // Arrange
            var providerBet = new ProviderBet();

            // Act & Assert - Multiple assignments to same property
            providerBet.Name = "First Name";
            providerBet.Name.Should().Be("First Name");

            providerBet.Name = "Second Name";
            providerBet.Name.Should().Be("Second Name");

            providerBet.Name = null;
            providerBet.Name.Should().BeNull();

            providerBet.Name = "Final Name";
            providerBet.Name.Should().Be("Final Name");
        }

        [Fact]
        public void ProviderBet_PropertyIndependence_ShouldNotAffectOtherProperties()
        {
            // Arrange
            var providerBet = new ProviderBet
            {
                Id = 100,
                Name = "Test Bet",
                                 Status = BetStatus.Open,
                 Price = "2.00",
                Probability = 0.50,
                LastUpdate = DateTime.Now
            };

            var originalValues = new
            {
                Id = providerBet.Id,
                Name = providerBet.Name,
                Status = providerBet.Status,
                Price = providerBet.Price,
                Probability = providerBet.Probability,
                LastUpdate = providerBet.LastUpdate
            };

            // Act - Change one property
            providerBet.Name = "Changed Name";

            // Assert - Other properties should remain unchanged
            providerBet.Id.Should().Be(originalValues.Id);
            providerBet.Status.Should().Be(originalValues.Status);
            providerBet.Price.Should().Be(originalValues.Price);
            providerBet.Probability.Should().Be(originalValues.Probability);
            providerBet.LastUpdate.Should().Be(originalValues.LastUpdate);
            
            // Only the changed property should be different
            providerBet.Name.Should().Be("Changed Name");
            providerBet.Name.Should().NotBe(originalValues.Name);
        }

        #endregion

        #region Type Tests

        [Fact]
        public void ProviderBet_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(ProviderBet);

            // Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
        }

        [Fact]
        public void ProviderBet_ShouldHaveParameterlessConstructor()
        {
            // Arrange
            var type = typeof(ProviderBet);

            // Act
            var constructor = type.GetConstructor(Type.EmptyTypes);

            // Assert
            constructor.Should().NotBeNull();
            constructor!.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProviderBet_ShouldBeInCorrectNamespace()
        {
            // Arrange
            var type = typeof(ProviderBet);

            // Assert
            type.Namespace.Should().Be("Trade360SDK.Common.Entities.Markets");
        }



        #endregion

        #region Polymorphism Tests

        [Fact]
        public void ProviderBet_ShouldBeUsableAsBaseBet()
        {
            // Arrange
            BaseBet baseBet = new ProviderBet();

            // Act & Assert
            baseBet.Should().BeOfType<ProviderBet>();
            baseBet.Should().BeAssignableTo<BaseBet>();
            
            // Test polymorphic property access
            baseBet.Id = 456;
            baseBet.Name = "Polymorphic Test";
            baseBet.Price = "3.00";
            
            baseBet.Id.Should().Be(456);
            baseBet.Name.Should().Be("Polymorphic Test");
            baseBet.Price.Should().Be("3.00");
        }

        [Fact]
        public void ProviderBet_CastFromBaseBet_ShouldWork()
        {
            // Arrange
            BaseBet baseBet = new ProviderBet { Id = 789, Name = "Cast Test" };

            // Act
            var providerBet = baseBet as ProviderBet;

            // Assert
            providerBet.Should().NotBeNull();
            providerBet!.Id.Should().Be(789);
            providerBet.Name.Should().Be("Cast Test");
        }

        #endregion
    }
} 