using Trade360SDK.CustomersApi.Entities.Base;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.Base
{
    public class BaseRequestTests
    {
        [Fact]
        public void CanInstantiateBaseRequest()
        {
            var request = new BaseRequest();
            Assert.NotNull(request);
        }
    }
} 