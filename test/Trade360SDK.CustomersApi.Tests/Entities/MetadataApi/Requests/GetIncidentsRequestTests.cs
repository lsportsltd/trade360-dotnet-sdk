using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Requests
{
    public class GetIncidentsRequestTests
    {
        [Fact]
        public void CanInstantiateGetIncidentsRequest()
        {
            var request = new GetIncidentsRequest();
            Assert.NotNull(request);
        }
    }
} 