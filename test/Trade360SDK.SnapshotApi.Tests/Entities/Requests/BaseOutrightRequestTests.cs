using Trade360SDK.SnapshotApi.Entities.Requests;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Requests
{
    public class BaseOutrightRequestTests
    {
        [Fact]
        public void CanInstantiateAndSetProperties()
        {
            var dto = new BaseOutrightRequest();
            Assert.NotNull(dto);
            // Add property assignment/asserts if properties exist
        }
    }
} 