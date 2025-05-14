using Trade360SDK.Feed.Configuration;
using Xunit;

namespace Trade360SDK.Feed.Tests
{
    public class RmqConnectionSettingsTests
    {
        [Fact]
        public void Defaults_Are_Correct()
        {
            var settings = new RmqConnectionSettings();
            Assert.Equal(100, settings.PrefetchCount);
            Assert.True(settings.DispatchConsumersAsync);
            Assert.True(settings.AutomaticRecoveryEnabled);
            Assert.True(settings.AutoAck);
            Assert.Equal(30, settings.RequestedHeartbeatSeconds);
            Assert.Equal(30, settings.NetworkRecoveryInterval);
        }
    }
} 