using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Requests
{
    public class GetIncidentsRequestDtoTests
    {
        [Fact]
        public void CanInstantiateGetIncidentsRequestDto()
        {
            var dto = new GetIncidentsRequestDto();
            Assert.NotNull(dto);
        }
    }
} 