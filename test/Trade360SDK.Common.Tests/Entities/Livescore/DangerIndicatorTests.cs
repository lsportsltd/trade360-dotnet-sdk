using System;
using Trade360SDK.Common.Entities.Livescore;
using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class DangerIndicatorTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var now = DateTime.UtcNow;
            var indicator = new DangerIndicator
            {
                Type = DangerIndicatorType.General,
                Status = DangerIndicatorStatus.Safe,
                LastUpdate = now
            };
            Assert.Equal(DangerIndicatorType.General, indicator.Type);
            Assert.Equal(DangerIndicatorStatus.Safe, indicator.Status);
            Assert.Equal(now, indicator.LastUpdate);
        }

        [Fact]
        public void Properties_ShouldAllowDefaults()
        {
            var indicator = new DangerIndicator();
            Assert.Equal(default, indicator.Type);
            Assert.Equal(default, indicator.Status);
            Assert.Equal(default, indicator.LastUpdate);
        }
    }
} 