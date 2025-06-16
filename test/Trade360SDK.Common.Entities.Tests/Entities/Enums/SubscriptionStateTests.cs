using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.Enums
{
    public class SubscriptionStateTests
    {
        [Fact]
        public void Enum_ShouldContainExpectedValues()
        {
            Assert.True(System.Enum.IsDefined(typeof(SubscriptionState), SubscriptionState.All));
            Assert.True(System.Enum.IsDefined(typeof(SubscriptionState), SubscriptionState.NotSubscribed));
            Assert.True(System.Enum.IsDefined(typeof(SubscriptionState), SubscriptionState.Subscribed));
        }
    }
} 