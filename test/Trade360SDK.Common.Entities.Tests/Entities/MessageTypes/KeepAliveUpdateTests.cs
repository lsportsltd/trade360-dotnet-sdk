using Trade360SDK.Common.Entities.KeepAlive;
using Trade360SDK.Common.Entities.MessageTypes;
using Xunit;
using KeepAliveEntity = Trade360SDK.Common.Entities.KeepAlive.KeepAlive;

namespace Trade360SDK.Common.Tests.Entities.MessageTypes
{
    public class KeepAliveUpdateTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var keepAlive = new KeepAliveEntity();
            var update = new KeepAliveUpdate
            {
                KeepAlive = keepAlive
            };
            Assert.Equal(keepAlive, update.KeepAlive);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var update = new KeepAliveUpdate();
            Assert.Null(update.KeepAlive);
        }
    }
} 