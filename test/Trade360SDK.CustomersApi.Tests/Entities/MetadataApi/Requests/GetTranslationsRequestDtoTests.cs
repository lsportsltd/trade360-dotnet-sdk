using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Requests
{
    public class GetTranslationsRequestDtoTests
    {
        [Fact]
        public void CanInstantiateGetTranslationsRequestDto()
        {
            var dto = new GetTranslationsRequestDto();
            Assert.NotNull(dto);
        }
    }
} 