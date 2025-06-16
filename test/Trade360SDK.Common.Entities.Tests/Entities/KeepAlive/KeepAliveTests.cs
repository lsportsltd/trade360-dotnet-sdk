using System.Collections.Generic;
using Trade360SDK.Common.Entities.KeepAlive;
using Trade360SDK.Common.Entities.Shared;
using Xunit;
using KeepAliveEntity = Trade360SDK.Common.Entities.KeepAlive.KeepAlive;

namespace Trade360SDK.Common.Tests.Entities.KeepAlive
{
    public class KeepAliveTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var activeEvents = new List<int> { 1, 2, 3 };
            var extraData = new List<NameValuePair> { new NameValuePair() };
            var keepAlive = new KeepAliveEntity
            {
                ActiveEvents = activeEvents,
                ExtraData = extraData,
                ProviderId = 42
            };
            Assert.Equal(activeEvents, keepAlive.ActiveEvents);
            Assert.Equal(extraData, keepAlive.ExtraData);
            Assert.Equal(42, keepAlive.ProviderId);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var keepAlive = new KeepAliveEntity();
            Assert.Null(keepAlive.ActiveEvents);
            Assert.Null(keepAlive.ExtraData);
            Assert.Null(keepAlive.ProviderId);
        }
    }
} 