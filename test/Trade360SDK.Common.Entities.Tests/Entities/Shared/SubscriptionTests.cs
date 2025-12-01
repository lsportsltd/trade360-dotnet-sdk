using Trade360SDK.Common.Entities.Shared;
using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class SubscriptionTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var subscription = new Subscription
            {
                Type = 2,
                Status = SubscriptionStatus.Subscribed
            };
            Assert.Equal(2, subscription.Type);
            Assert.Equal(SubscriptionStatus.Subscribed, subscription.Status);
        }

        [Fact]
        public void Properties_ShouldHaveDefaultValues()
        {
            var subscription = new Subscription();
            Assert.Equal(0, subscription.Type); // default int
            Assert.Equal(SubscriptionStatus.NotSet, subscription.Status); // default enum
        }
    }
} 