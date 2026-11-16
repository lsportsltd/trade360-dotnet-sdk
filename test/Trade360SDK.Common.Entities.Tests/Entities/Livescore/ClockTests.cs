using System.Text.Json;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Livescore;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class ClockTests
    {
        [Fact]
        public void StoppageTimeSeconds_RoundTrips_AndIsOmittedWhenNull()
        {
            var clock = new Clock
            {
                Status = ClockStatus.Running,
                Seconds = 5400,
                StoppageTimeSeconds = 300
            };

            var json = JsonSerializer.Serialize(clock);
            Assert.Contains("\"StoppageTimeSeconds\":300", json);

            var cleared = new Clock
            {
                Status = ClockStatus.Running,
                Seconds = 5400
            };

            var clearedJson = JsonSerializer.Serialize(cleared);
            Assert.DoesNotContain("StoppageTimeSeconds", clearedJson);

            var read = JsonSerializer.Deserialize<Clock>(json);
            Assert.Equal(300, read!.StoppageTimeSeconds);
        }
    }
}
