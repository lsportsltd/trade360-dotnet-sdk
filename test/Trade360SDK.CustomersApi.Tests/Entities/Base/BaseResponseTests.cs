using Trade360SDK.CustomersApi.Entities.Base;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.Base
{
    public class BaseResponseTests
    {
        [Fact]
        public void CanInstantiateBaseResponse()
        {
            var response = new BaseResponse<object>();
            Assert.NotNull(response);
        }
    }
} 