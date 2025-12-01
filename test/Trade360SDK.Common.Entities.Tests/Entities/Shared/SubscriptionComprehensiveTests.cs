using System;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Entities.Shared;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Tests.Entities.Shared
{
    public class SubscriptionComprehensiveTests
    {
        [Fact]
        public void Subscription_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Act
            var subscription = new Subscription();

            // Assert
            subscription.Should().NotBeNull();
            subscription.Type.Should().Be(0);
            subscription.Status.Should().Be(SubscriptionStatus.NotSet); // Default enum value
        }

        [Fact]
        public void Subscription_SetType_ShouldSetValue()
        {
            // Arrange
            var subscription = new Subscription();
            var type = 5;

            // Act
            subscription.Type = type;

            // Assert
            subscription.Type.Should().Be(type);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(999)]
        [InlineData(12345)]
        public void Subscription_SetVariousTypes_ShouldSetValue(int type)
        {
            // Arrange
            var subscription = new Subscription();

            // Act
            subscription.Type = type;

            // Assert
            subscription.Type.Should().Be(type);
        }

        [Fact]
        public void Subscription_SetStatus_ShouldSetValue()
        {
            // Arrange
            var subscription = new Subscription();
            var status = SubscriptionStatus.Subscribed;

            // Act
            subscription.Status = status;

            // Assert
            subscription.Status.Should().Be(status);
        }

        [Theory]
        [InlineData(SubscriptionStatus.NotSet)]
        [InlineData(SubscriptionStatus.Subscribed)]
        [InlineData(SubscriptionStatus.Pending)]
        [InlineData(SubscriptionStatus.Unsubscribed)]
        [InlineData(SubscriptionStatus.Deleted)]
        public void Subscription_SetVariousStatuses_ShouldSetValue(SubscriptionStatus status)
        {
            // Arrange
            var subscription = new Subscription();

            // Act
            subscription.Status = status;

            // Assert
            subscription.Status.Should().Be(status);
        }

        [Fact]
        public void Subscription_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var subscription = new Subscription();
            var type = 100;
            var status = SubscriptionStatus.Subscribed;

            // Act
            subscription.Type = type;
            subscription.Status = status;

            // Assert
            subscription.Type.Should().Be(type);
            subscription.Status.Should().Be(status);
        }

        [Fact]
        public void Subscription_PropertySettersGetters_ShouldWorkCorrectly()
        {
            // Arrange
            var subscription = new Subscription();

            // Act & Assert - Test that we can set and get each property multiple times
            subscription.Type = 1;
            subscription.Type.Should().Be(1);
            subscription.Type = 2;
            subscription.Type.Should().Be(2);

            subscription.Status = SubscriptionStatus.NotSet;
            subscription.Status.Should().Be(SubscriptionStatus.NotSet);
            subscription.Status = SubscriptionStatus.Subscribed;
            subscription.Status.Should().Be(SubscriptionStatus.Subscribed);
            subscription.Status = SubscriptionStatus.Unsubscribed;
            subscription.Status.Should().Be(SubscriptionStatus.Unsubscribed);
        }

        [Fact]
        public void Subscription_WithRealWorldData_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var activeSubscription = new Subscription
            {
                Type = 1,
                Status = SubscriptionStatus.Subscribed
            };

            var pendingSubscription = new Subscription
            {
                Type = 2,
                Status = SubscriptionStatus.Pending
            };

            var inactiveSubscription = new Subscription
            {
                Type = 3,
                Status = SubscriptionStatus.Unsubscribed
            };

            // Assert
            activeSubscription.Type.Should().Be(1);
            activeSubscription.Status.Should().Be(SubscriptionStatus.Subscribed);

            pendingSubscription.Type.Should().Be(2);
            pendingSubscription.Status.Should().Be(SubscriptionStatus.Pending);

            inactiveSubscription.Type.Should().Be(3);
            inactiveSubscription.Status.Should().Be(SubscriptionStatus.Unsubscribed);
        }

        [Fact]
        public void Subscription_WithNotSetStatus_ShouldRepresentUnknownState()
        {
            // Arrange & Act
            var subscription = new Subscription
            {
                Type = 1,
                Status = SubscriptionStatus.NotSet
            };

            // Assert
            subscription.Status.Should().Be(SubscriptionStatus.NotSet);
            subscription.Status.Should().Be((SubscriptionStatus)0);
        }

        [Fact]
        public void Subscription_WithDeletedStatus_ShouldRepresentDeletedState()
        {
            // Arrange & Act
            var subscription = new Subscription
            {
                Type = 999,
                Status = SubscriptionStatus.Deleted
            };

            // Assert
            subscription.Status.Should().Be(SubscriptionStatus.Deleted);
            subscription.Type.Should().Be(999);
        }

        [Fact]
        public void Subscription_ToString_ShouldReturnStringRepresentation()
        {
            // Arrange
            var subscription = new Subscription
            {
                Type = 1,
                Status = SubscriptionStatus.Subscribed
            };

            // Act
            var result = subscription.ToString();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("Subscription");
        }

        [Fact]
        public void Subscription_GetHashCode_ShouldReturnConsistentValue()
        {
            // Arrange
            var subscription = new Subscription
            {
                Type = 1,
                Status = SubscriptionStatus.Subscribed
            };

            // Act
            var hashCode1 = subscription.GetHashCode();
            var hashCode2 = subscription.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void Subscription_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var subscription = new Subscription();

            // Assert
            subscription.GetType().Should().Be(typeof(Subscription));
            subscription.GetType().Name.Should().Be("Subscription");
            subscription.GetType().Namespace.Should().Be("Trade360SDK.Common.Entities.Shared");
        }

        [Fact]
        public void Subscription_Properties_ShouldHaveCorrectTypes()
        {
            // Arrange & Act
            var subscription = new Subscription();

            // Assert
            subscription.Type.GetType().Should().Be(typeof(int));
            subscription.Status.GetType().Should().Be(typeof(SubscriptionStatus));
            subscription.Status.GetType().IsEnum.Should().BeTrue();
        }

        [Fact]
        public void Subscription_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange & Act
            var subscription1 = new Subscription { Type = 1, Status = SubscriptionStatus.Subscribed };
            var subscription2 = new Subscription { Type = 2, Status = SubscriptionStatus.Pending };

            // Assert
            subscription1.Type.Should().NotBe(subscription2.Type);
            subscription1.Status.Should().NotBe(subscription2.Status);
            subscription1.Should().NotBeSameAs(subscription2);
        }

        [Fact]
        public void Subscription_StatusEnum_ShouldHaveCorrectValues()
        {
            // Assert
            ((int)SubscriptionStatus.NotSet).Should().Be(0);
            ((int)SubscriptionStatus.Subscribed).Should().Be(1);
            ((int)SubscriptionStatus.Pending).Should().Be(2);
            ((int)SubscriptionStatus.Unsubscribed).Should().Be(3);
            ((int)SubscriptionStatus.Deleted).Should().Be(4);
        }

        [Fact]
        public void Subscription_StatusEnum_ShouldConvertToStringCorrectly()
        {
            // Arrange & Act & Assert
            SubscriptionStatus.NotSet.ToString().Should().Be("NotSet");
            SubscriptionStatus.Subscribed.ToString().Should().Be("Subscribed");
            SubscriptionStatus.Pending.ToString().Should().Be("Pending");
            SubscriptionStatus.Unsubscribed.ToString().Should().Be("Unsubscribed");
            SubscriptionStatus.Deleted.ToString().Should().Be("Deleted");
        }

        [Fact]
        public void Subscription_StatusEnum_ShouldParseFromStringCorrectly()
        {
            // Arrange & Act & Assert
            Enum.Parse<SubscriptionStatus>("NotSet").Should().Be(SubscriptionStatus.NotSet);
            Enum.Parse<SubscriptionStatus>("Subscribed").Should().Be(SubscriptionStatus.Subscribed);
            Enum.Parse<SubscriptionStatus>("Pending").Should().Be(SubscriptionStatus.Pending);
            Enum.Parse<SubscriptionStatus>("Unsubscribed").Should().Be(SubscriptionStatus.Unsubscribed);
            Enum.Parse<SubscriptionStatus>("Deleted").Should().Be(SubscriptionStatus.Deleted);
        }

        [Fact]
        public void Subscription_StatusEnum_ShouldParseFromIntegerCorrectly()
        {
            // Arrange & Act & Assert
            ((SubscriptionStatus)0).Should().Be(SubscriptionStatus.NotSet);
            ((SubscriptionStatus)1).Should().Be(SubscriptionStatus.Subscribed);
            ((SubscriptionStatus)2).Should().Be(SubscriptionStatus.Pending);
            ((SubscriptionStatus)3).Should().Be(SubscriptionStatus.Unsubscribed);
            ((SubscriptionStatus)4).Should().Be(SubscriptionStatus.Deleted);
        }

        [Fact]
        public void Subscription_StatusEnum_ShouldHandleInvalidValues()
        {
            // Arrange & Act
            var invalidStatus = (SubscriptionStatus)999;

            // Assert
            invalidStatus.Should().Be((SubscriptionStatus)999);
            ((int)invalidStatus).Should().Be(999);
        }

        [Fact]
        public void Subscription_WithNegativeType_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var subscription = new Subscription
            {
                Type = -100,
                Status = SubscriptionStatus.Subscribed
            };

            // Assert
            subscription.Type.Should().Be(-100);
            subscription.Status.Should().Be(SubscriptionStatus.Subscribed);
        }

        [Fact]
        public void Subscription_WithMaxIntType_ShouldStoreCorrectly()
        {
            // Arrange & Act
            var subscription = new Subscription
            {
                Type = int.MaxValue,
                Status = SubscriptionStatus.Deleted
            };

            // Assert
            subscription.Type.Should().Be(int.MaxValue);
            subscription.Status.Should().Be(SubscriptionStatus.Deleted);
        }

        [Fact]
        public void Subscription_DefaultStatus_ShouldBeNotSet()
        {
            // Arrange & Act
            var subscription = new Subscription();

            // Assert
            subscription.Status.Should().Be(SubscriptionStatus.NotSet);
            subscription.Status.Should().Be((SubscriptionStatus)0);
        }
    }
} 