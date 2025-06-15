using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.Enums
{
    public class SubscriptionStatusTests
    {
        [Fact]
        public void Enum_ShouldContainExpectedValues()
        {
            Assert.True(System.Enum.IsDefined(typeof(SubscriptionStatus), SubscriptionStatus.NotSet));
            Assert.True(System.Enum.IsDefined(typeof(SubscriptionStatus), SubscriptionStatus.Subscribed));
            Assert.True(System.Enum.IsDefined(typeof(SubscriptionStatus), SubscriptionStatus.Pending));
            Assert.True(System.Enum.IsDefined(typeof(SubscriptionStatus), SubscriptionStatus.Unsubscribed));
            Assert.True(System.Enum.IsDefined(typeof(SubscriptionStatus), SubscriptionStatus.Deleted));
        }
    }
} 