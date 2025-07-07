using FluentAssertions;
using Trade360SDK.Common.Entities.Fixtures;
using Xunit;
using System;

namespace Trade360SDK.Common.Tests
{
    public class FixtureEventTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstanceSuccessfully()
        {
            // Act
            var fixtureEvent = new FixtureEvent();

            // Assert
            fixtureEvent.Should().NotBeNull();
            fixtureEvent.FixtureId.Should().Be(0);
            fixtureEvent.Fixture.Should().BeNull();
        }

        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            // Arrange
            var fixture = new Fixture();
            var evt = new FixtureEvent();

            // Act
            evt.FixtureId = 42;
            evt.Fixture = fixture;

            // Assert
            evt.FixtureId.Should().Be(42);
            evt.Fixture.Should().BeSameAs(fixture);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            // Arrange & Act
            var evt = new FixtureEvent();

            // Assert
            evt.FixtureId.Should().Be(0);
            evt.Fixture.Should().BeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999999)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void FixtureId_ShouldHandleVariousIntegerValues(int fixtureId)
        {
            // Arrange
            var evt = new FixtureEvent();

            // Act
            evt.FixtureId = fixtureId;

            // Assert
            evt.FixtureId.Should().Be(fixtureId);
        }

        [Fact]
        public void Fixture_ShouldAllowNullAssignment()
        {
            // Arrange
            var evt = new FixtureEvent();
            var fixture = new Fixture();
            evt.Fixture = fixture;

            // Act
            evt.Fixture = null;

            // Assert
            evt.Fixture.Should().BeNull();
        }

        [Fact]
        public void Fixture_ShouldAllowReassignment()
        {
            // Arrange
            var evt = new FixtureEvent();
            var firstFixture = new Fixture();
            var secondFixture = new Fixture();

            // Act
            evt.Fixture = firstFixture;
            evt.Fixture = secondFixture;

            // Assert
            evt.Fixture.Should().BeSameAs(secondFixture);
            evt.Fixture.Should().NotBeSameAs(firstFixture);
        }

        [Fact]
        public void Properties_ShouldBeIndependent()
        {
            // Arrange
            var evt = new FixtureEvent();
            var fixture = new Fixture();

            // Act
            evt.FixtureId = 123;
            evt.Fixture = fixture;

            // Assert
            evt.FixtureId.Should().Be(123);
            evt.Fixture.Should().BeSameAs(fixture);

            // Modify one property, other should remain unchanged
            evt.FixtureId = 456;
            evt.Fixture.Should().BeSameAs(fixture);

            evt.Fixture = null;
            evt.FixtureId.Should().Be(456);
        }

        [Fact]
        public void FixtureEvent_ShouldSupportObjectInitializer()
        {
            // Arrange & Act
            var evt = new FixtureEvent
            {
                FixtureId = 789,
                Fixture = new Fixture()
            };

            // Assert
            evt.FixtureId.Should().Be(789);
            evt.Fixture.Should().NotBeNull();
        }

        [Fact]
        public void FixtureEvent_ShouldSupportReflectionAccess()
        {
            // Arrange
            var evt = new FixtureEvent();
            var type = typeof(FixtureEvent);
            var fixtureIdProperty = type.GetProperty("FixtureId");
            var fixtureProperty = type.GetProperty("Fixture");
            var fixture = new Fixture();

            // Act
            fixtureIdProperty!.SetValue(evt, 999);
            fixtureProperty!.SetValue(evt, fixture);

            // Assert
            var fixtureIdValue = (int)fixtureIdProperty.GetValue(evt)!;
            var fixtureValue = (Fixture?)fixtureProperty.GetValue(evt);

            fixtureIdValue.Should().Be(999);
            fixtureValue.Should().BeSameAs(fixture);
        }

        [Fact]
        public void FixtureEvent_ShouldBeInCorrectNamespace()
        {
            // Arrange
            var type = typeof(FixtureEvent);

            // Act & Assert
            type.Namespace.Should().Be("Trade360SDK.Common.Entities.Fixtures");
            type.FullName.Should().Be("Trade360SDK.Common.Entities.Fixtures.FixtureEvent");
        }

        [Fact]
        public void FixtureEvent_ShouldHaveCorrectPropertyTypes()
        {
            // Arrange
            var type = typeof(FixtureEvent);
            var fixtureIdProperty = type.GetProperty("FixtureId");
            var fixtureProperty = type.GetProperty("Fixture");

            // Act & Assert
            fixtureIdProperty.Should().NotBeNull();
            fixtureIdProperty!.PropertyType.Should().Be(typeof(int));

            fixtureProperty.Should().NotBeNull();
            fixtureProperty!.PropertyType.Should().Be(typeof(Fixture));
        }

        [Fact]
        public void FixtureEvent_WithComplexFixture_ShouldMaintainIntegrity()
        {
            // Arrange
            var evt = new FixtureEvent();
            var fixture = new Fixture
            {
                Sport = new Sport { Id = 1, Name = "Football" },
                League = new League { Id = 10, Name = "Premier League" },
                StartDate = new DateTime(2024, 1, 1, 15, 0, 0, DateTimeKind.Utc)
            };

            // Act
            evt.FixtureId = 12345;
            evt.Fixture = fixture;

            // Assert
            evt.FixtureId.Should().Be(12345);
            evt.Fixture.Should().NotBeNull();
            evt.Fixture!.Sport.Should().NotBeNull();
            evt.Fixture.Sport!.Name.Should().Be("Football");
            evt.Fixture.League.Should().NotBeNull();
            evt.Fixture.League!.Name.Should().Be("Premier League");
        }

        [Fact]
        public void FixtureEvent_ShouldHandleEmptyFixture()
        {
            // Arrange
            var evt = new FixtureEvent();
            var emptyFixture = new Fixture(); // All properties null/default

            // Act
            evt.FixtureId = 100;
            evt.Fixture = emptyFixture;

            // Assert
            evt.FixtureId.Should().Be(100);
            evt.Fixture.Should().BeSameAs(emptyFixture);
            evt.Fixture.Sport.Should().BeNull();
            evt.Fixture.League.Should().BeNull();
            evt.Fixture.StartDate.Should().BeNull();
        }

        [Fact]
        public void FixtureEvent_ShouldHandleExtremeFixtureIds()
        {
            // Arrange
            var evt = new FixtureEvent();

            // Act & Assert - Maximum value
            evt.FixtureId = int.MaxValue;
            evt.FixtureId.Should().Be(int.MaxValue);

            // Act & Assert - Minimum value
            evt.FixtureId = int.MinValue;
            evt.FixtureId.Should().Be(int.MinValue);

            // Act & Assert - Zero
            evt.FixtureId = 0;
            evt.FixtureId.Should().Be(0);
        }

        [Fact]
        public void FixtureEvent_PropertyAssignment_ShouldBeAtomic()
        {
            // Arrange
            var evt = new FixtureEvent();
            var fixture1 = new Fixture { Sport = new Sport { Id = 1, Name = "Sport1" } };
            var fixture2 = new Fixture { Sport = new Sport { Id = 2, Name = "Sport2" } };

            // Act
            evt.FixtureId = 500;
            evt.Fixture = fixture1;

            // Verify initial state
            evt.FixtureId.Should().Be(500);
            evt.Fixture.Should().BeSameAs(fixture1);

            // Act - Change both properties
            evt.FixtureId = 600;
            evt.Fixture = fixture2;

            // Assert - Both changes should be preserved
            evt.FixtureId.Should().Be(600);
            evt.Fixture.Should().BeSameAs(fixture2);
            evt.Fixture.Should().NotBeSameAs(fixture1);
        }

        [Fact]
        public void FixtureEvent_MultipleInstancesShouldBeIndependent()
        {
            // Arrange
            var evt1 = new FixtureEvent();
            var evt2 = new FixtureEvent();
            var fixture1 = new Fixture();
            var fixture2 = new Fixture();

            // Act
            evt1.FixtureId = 111;
            evt1.Fixture = fixture1;

            evt2.FixtureId = 222;
            evt2.Fixture = fixture2;

            // Assert
            evt1.FixtureId.Should().Be(111);
            evt1.Fixture.Should().BeSameAs(fixture1);

            evt2.FixtureId.Should().Be(222);
            evt2.Fixture.Should().BeSameAs(fixture2);

            // Instances should be completely independent
            evt1.Fixture.Should().NotBeSameAs(evt2.Fixture);
        }

        [Fact]
        public void FixtureEvent_ShouldSupportZeroFixtureId()
        {
            // Arrange
            var evt = new FixtureEvent();
            var fixture = new Fixture();

            // Act
            evt.FixtureId = 0; // Explicitly set to zero
            evt.Fixture = fixture;

            // Assert
            evt.FixtureId.Should().Be(0);
            evt.Fixture.Should().BeSameAs(fixture);
        }

        [Fact]
        public void FixtureEvent_TypeInformation_ShouldBeCorrect()
        {
            // Arrange
            var evt = new FixtureEvent();
            var type = evt.GetType();

            // Act & Assert
            type.Name.Should().Be("FixtureEvent");
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
            type.IsSealed.Should().BeFalse();
            type.IsPublic.Should().BeTrue();
        }
    }
} 