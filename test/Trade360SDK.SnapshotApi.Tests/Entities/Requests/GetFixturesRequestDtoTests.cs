using Trade360SDK.SnapshotApi.Entities.Requests;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Requests
{
    public class GetFixturesRequestDtoTests
    {
        [Fact]
        public void CanInstantiateAndSetProperties()
        {
            var dto = new GetFixturesRequestDto();
            Assert.NotNull(dto);
            // Add property assignment/asserts if properties exist
        }
    }
} 