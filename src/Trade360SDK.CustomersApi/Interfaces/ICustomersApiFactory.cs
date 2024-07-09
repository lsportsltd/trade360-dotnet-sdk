using Trade360SDK.CustomersApi.Configuration;

namespace Trade360SDK.CustomersApi.Interfaces
{
    public interface ICustomersApiFactory
    {
        IMetadataApiClient CreateMetadataHttpClient(CustomersApiSettings settings);
        IPackageDistributionApiClient CreatePackageDistributionHttpClient(CustomersApiSettings settings);
        ISubscriptionApiClient CreateSubscriptionHttpClient(CustomersApiSettings settings);
    }
}
