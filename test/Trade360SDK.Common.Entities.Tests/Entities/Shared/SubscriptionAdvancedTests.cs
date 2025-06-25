using System;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Entities.Shared;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Tests.Entities.Shared
{
    public class SubscriptionAdvancedTests
    {
        [Fact]
        public void Subscription_Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var subscription = new Subscription();

            // Assert
            subscription.Should().NotBeNull();
            subscription.Type.Should().Be(0);
            subscription.Status.Should().Be(SubscriptionStatus.NotSet);
        }

        [Fact]
        public void Subscription_SetType_ShouldAcceptValidInteger()
        {
            // Arrange
            var subscription = new Subscription();
            var testType = 1;

            // Act
            subscription.Type = testType;

            // Assert
            subscription.Type.Should().Be(testType);
        }

        [Fact]
        public void Subscription_SetStatus_ShouldAcceptValidEnum()
        {
            // Arrange
            var subscription = new Subscription();
            var testStatus = SubscriptionStatus.Subscribed;

            // Act
            subscription.Status = testStatus;

            // Assert
            subscription.Status.Should().Be(testStatus);
        }

        [Theory]
        [InlineData(1, SubscriptionStatus.Subscribed)]
        [InlineData(2, SubscriptionStatus.Pending)]
        [InlineData(0, SubscriptionStatus.NotSet)]
        [InlineData(-1, SubscriptionStatus.Unsubscribed)]
        [InlineData(999, SubscriptionStatus.Deleted)]
        public void Subscription_SetBothProperties_ShouldStoreValues(int type, SubscriptionStatus status)
        {
            // Arrange
            var subscription = new Subscription();

            // Act
            subscription.Type = type;
            subscription.Status = status;

            // Assert
            subscription.Type.Should().Be(type);
            subscription.Status.Should().Be(status);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(999999)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Subscription_SetType_ShouldAcceptVariousIntegers(int type)
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
        public void Subscription_SetStatus_ShouldAcceptAllEnumValues(SubscriptionStatus status)
        {
            // Arrange
            var subscription = new Subscription();

            // Act
            subscription.Status = status;

            // Assert
            subscription.Status.Should().Be(status);
        }

        [Fact]
        public void Subscription_PropertyAccessMultipleTimes_ShouldReturnSameValue()
        {
            // Arrange
            var subscription = new Subscription
            {
                Type = 5,
                Status = SubscriptionStatus.Subscribed
            };

            // Act & Assert
            subscription.Type.Should().Be(5);
            subscription.Type.Should().Be(5);
            subscription.Status.Should().Be(SubscriptionStatus.Subscribed);
            subscription.Status.Should().Be(SubscriptionStatus.Subscribed);
        }



        [Fact]
        public void Subscription_WithBusinessScenarios_ShouldWorkCorrectly()
        {
            // Arrange & Act
            var inPlaySubscription = new Subscription
            {
                Type = 1,
                Status = SubscriptionStatus.Subscribed
            };

            var preMatchSubscription = new Subscription
            {
                Type = 2,
                Status = SubscriptionStatus.Unsubscribed
            };

            var liveScoreSubscription = new Subscription
            {
                Type = 3,
                Status = SubscriptionStatus.Pending
            };

            // Assert
            inPlaySubscription.Type.Should().Be(1);
            inPlaySubscription.Status.Should().Be(SubscriptionStatus.Subscribed);

            preMatchSubscription.Type.Should().Be(2);
            preMatchSubscription.Status.Should().Be(SubscriptionStatus.Unsubscribed);

            liveScoreSubscription.Type.Should().Be(3);
            liveScoreSubscription.Status.Should().Be(SubscriptionStatus.Pending);
        }



        [Fact]
        public void Subscription_DefaultValues_ShouldBeCorrect()
        {
            // Act
            var subscription = new Subscription();

            // Assert
            subscription.Type.Should().Be(0);
            subscription.Status.Should().Be(SubscriptionStatus.NotSet);
        }

        [Fact]
        public void Subscription_SetAndResetValues_ShouldWorkCorrectly()
        {
            // Arrange
            var subscription = new Subscription();

            // Act & Assert - Set values
            subscription.Type = 100;
            subscription.Status = SubscriptionStatus.Subscribed;
            subscription.Type.Should().Be(100);
            subscription.Status.Should().Be(SubscriptionStatus.Subscribed);

            // Act & Assert - Change values
            subscription.Type = 200;
            subscription.Status = SubscriptionStatus.Unsubscribed;
            subscription.Type.Should().Be(200);
            subscription.Status.Should().Be(SubscriptionStatus.Unsubscribed);

            // Act & Assert - Reset to defaults
            subscription.Type = 0;
            subscription.Status = SubscriptionStatus.NotSet;
            subscription.Type.Should().Be(0);
            subscription.Status.Should().Be(SubscriptionStatus.NotSet);
        }

        [Fact]
        public void Subscription_WithNegativeType_ShouldStoreCorrectly()
        {
            // Arrange
            var subscription = new Subscription();

            // Act
            subscription.Type = -999;

            // Assert
            subscription.Type.Should().Be(-999);
        }

        [Fact]
        public void Subscription_WithAllEnumValues_ShouldWorkCorrectly()
        {
            // Arrange & Act
            var subscriptions = new[]
            {
                new Subscription { Type = 1, Status = SubscriptionStatus.NotSet },
                new Subscription { Type = 2, Status = SubscriptionStatus.Subscribed },
                new Subscription { Type = 3, Status = SubscriptionStatus.Unsubscribed },
                new Subscription { Type = 4, Status = SubscriptionStatus.Pending },
                new Subscription { Type = 5, Status = SubscriptionStatus.Deleted }
            };

            // Assert
            subscriptions[0].Status.Should().Be(SubscriptionStatus.NotSet);
            subscriptions[1].Status.Should().Be(SubscriptionStatus.Subscribed);
            subscriptions[2].Status.Should().Be(SubscriptionStatus.Unsubscribed);
            subscriptions[3].Status.Should().Be(SubscriptionStatus.Pending);
            subscriptions[4].Status.Should().Be(SubscriptionStatus.Deleted);
        }

        [Fact]
        public void Subscription_WithMaxIntValues_ShouldHandleCorrectly()
        {
            // Arrange
            var subscription = new Subscription();

            // Act
            subscription.Type = int.MaxValue;

            // Assert
            subscription.Type.Should().Be(int.MaxValue);
        }

        [Fact]
        public void Subscription_WithMinIntValues_ShouldHandleCorrectly()
        {
            // Arrange
            var subscription = new Subscription();

            // Act
            subscription.Type = int.MinValue;

            // Assert
            subscription.Type.Should().Be(int.MinValue);
        }


    }
} 