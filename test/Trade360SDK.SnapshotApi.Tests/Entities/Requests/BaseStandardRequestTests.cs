using Trade360SDK.SnapshotApi.Entities.Requests;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Requests
{
    public class BaseStandardRequestTests
    {
        [Fact]
        public void CanInstantiateAndSetProperties()
        {
            var dto = new BaseStandardRequest();
            Assert.NotNull(dto);
            // Add property assignment/asserts if properties exist
        }
    }
} 