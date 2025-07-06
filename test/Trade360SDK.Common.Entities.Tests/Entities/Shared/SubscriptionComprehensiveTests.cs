using FluentAssertions;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Shared;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.Shared
{
    public class SubscriptionComprehensiveTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var subscription = new Subscription();

            // Assert
            subscription.Type.Should().Be(default(int));
            subscription.Status.Should().Be(default(SubscriptionStatus));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Type_WithVariousValues_ShouldStoreCorrectly(int type)
        {
            // Arrange
            var subscription = new Subscription();

            // Act
            subscription.Type = type;

            // Assert
            subscription.Type.Should().Be(type);
        }

        [Theory]
        [InlineData(SubscriptionStatus.NotSet)]
        [InlineData(SubscriptionStatus.Subscribed)]
        [InlineData(SubscriptionStatus.Pending)]
        [InlineData(SubscriptionStatus.Unsubscribed)]
        [InlineData(SubscriptionStatus.Deleted)]
        public void Status_WithVariousValues_ShouldStoreCorrectly(SubscriptionStatus status)
        {
            // Arrange
            var subscription = new Subscription();

            // Act
            subscription.Status = status;

            // Assert
            subscription.Status.Should().Be(status);
        }

        [Fact]
        public void Properties_CanBeSetIndependently()
        {
            // Arrange
            var subscription = new Subscription();
            const int expectedType = 42;
            const SubscriptionStatus expectedStatus = SubscriptionStatus.Subscribed;

            // Act
            subscription.Type = expectedType;
            subscription.Status = expectedStatus;

            // Assert
            subscription.Type.Should().Be(expectedType);
            subscription.Status.Should().Be(expectedStatus);
        }

        [Fact]
        public void Properties_CanBeOverwritten()
        {
            // Arrange
            var subscription = new Subscription
            {
                Type = 1,
                Status = SubscriptionStatus.NotSet
            };

            const int newType = 999;
            const SubscriptionStatus newStatus = SubscriptionStatus.Deleted;

            // Act
            subscription.Type = newType;
            subscription.Status = newStatus;

            // Assert
            subscription.Type.Should().Be(newType);
            subscription.Status.Should().Be(newStatus);
        }

        [Fact]
        public void ObjectInitializer_ShouldSetAllProperties()
        {
            // Arrange
            const int expectedType = 123;
            const SubscriptionStatus expectedStatus = SubscriptionStatus.Pending;

            // Act
            var subscription = new Subscription
            {
                Type = expectedType,
                Status = expectedStatus
            };

            // Assert
            subscription.Type.Should().Be(expectedType);
            subscription.Status.Should().Be(expectedStatus);
        }

        [Fact]
        public void Type_WithZero_ShouldAcceptValue()
        {
            // Arrange
            var subscription = new Subscription();

            // Act
            subscription.Type = 0;

            // Assert
            subscription.Type.Should().Be(0);
        }

        [Fact]
        public void Type_WithNegativeValues_ShouldAcceptValue()
        {
            // Arrange
            var subscription = new Subscription();

            // Act
            subscription.Type = -100;

            // Assert
            subscription.Type.Should().Be(-100);
        }

        [Fact]
        public void Type_WithLargeValues_ShouldAcceptValue()
        {
            // Arrange
            var subscription = new Subscription();
            const int largeValue = 1000000;

            // Act
            subscription.Type = largeValue;

            // Assert
            subscription.Type.Should().Be(largeValue);
        }

        [Fact]
        public void Status_WithDefaultEnum_ShouldBe_NotSet()
        {
            // Arrange
            var subscription = new Subscription();

            // Act & Assert
            subscription.Status.Should().Be(SubscriptionStatus.NotSet);
            ((int)subscription.Status).Should().Be(0);
        }

        [Fact]
        public void AllProperties_CanBeAssignedSimultaneously()
        {
            // Arrange
            const int type = 456;
            const SubscriptionStatus status = SubscriptionStatus.Unsubscribed;

            // Act
            var subscription = new Subscription();
            subscription.Type = type;
            subscription.Status = status;

            // Assert
            subscription.Type.Should().Be(type);
            subscription.Status.Should().Be(status);
        }

        [Fact]
        public void ToString_ShouldNotThrow()
        {
            // Arrange
            var subscription = new Subscription
            {
                Type = 789,
                Status = SubscriptionStatus.Subscribed
            };

            // Act & Assert
            var act = () => subscription.ToString();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetHashCode_ShouldNotThrow()
        {
            // Arrange
            var subscription = new Subscription
            {
                Type = 321,
                Status = SubscriptionStatus.Pending
            };

            // Act & Assert
            var act = () => subscription.GetHashCode();
            act.Should().NotThrow();
        }

        [Fact]
        public void Equals_WithSameReference_ShouldReturnTrue()
        {
            // Arrange
            var subscription = new Subscription
            {
                Type = 654,
                Status = SubscriptionStatus.Deleted
            };

            // Act & Assert
            subscription.Equals(subscription).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithNull_ShouldReturnFalse()
        {
            // Arrange
            var subscription = new Subscription
            {
                Type = 987,
                Status = SubscriptionStatus.Unsubscribed
            };

            // Act & Assert
            subscription.Equals(null).Should().BeFalse();
        }
    }
} 