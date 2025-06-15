using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Requests
{
    public class GetTranslationsRequestTests
    {
        [Fact]
        public void CanInstantiateGetTranslationsRequest()
        {
            var request = new GetTranslationsRequest();
            Assert.NotNull(request);
        }
    }
} 