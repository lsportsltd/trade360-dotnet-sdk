namespace Trade360SDK.Api.Abstraction.Interfaces
{
    public interface ICustomersApiFactory
    {
        IMetadataApiClient CreateMetadataHttpClient(CustomersApiSettings settings);
        IPackageDistributionApiClient CreatePackageDistributionHttpClient(CustomersApiSettings settings);
        ISubscriptionApiClient CreateSubscriptionHttpClient(CustomersApiSettings settings);
    }
}
