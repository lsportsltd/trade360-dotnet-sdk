using Trade360SDK.Common.Configuration;

namespace Trade360SDK.CustomersApi.Interfaces
{
    public interface ICustomersApiFactory
    {
        IMetadataApiClient CreateMetadataHttpClient(string? baseUrl, PackageCredentials? packageCredentials);
        IPackageDistributionApiClient CreatePackageDistributionHttpClient(string? baseUrl, PackageCredentials? packageCredentials);
        ISubscriptionApiClient CreateSubscriptionHttpClient(string? baseUrl, PackageCredentials? packageCredentials);
    }
}
