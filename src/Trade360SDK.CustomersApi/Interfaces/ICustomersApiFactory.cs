using Trade360SDK.Common.Configuration;

namespace Trade360SDK.CustomersApi.Interfaces
{
    public interface ICustomersApiFactory
    {
        IMetadataHttpClient CreateMetadataHttpClient(string? baseUrl, PackageCredentials? packageCredentials);
        IPackageDistributionHttpClient CreatePackageDistributionHttpClient(string? baseUrl, PackageCredentials? packageCredentials);
        ISubscriptionHttpClient CreateSubscriptionHttpClient(string? baseUrl, PackageCredentials? packageCredentials);
        IPackageQueryHttpClient CreatePackageQueryHttpClient(string? baseUrl, PackageCredentials? packageCredentials);
    }
}
