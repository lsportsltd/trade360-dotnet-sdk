using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Interfaces;

namespace Trade360SDK.CustomersApi
{
    public class CustomersApiFactory : ICustomersApiFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomersApiFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IMetadataApiClient CreateMetadataHttpClient(string? baseUrl, PackageCredentials? packageCredentials)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            return new MetadataApiClient(httpClientFactory, baseUrl, packageCredentials, mapper);
        }

        public IPackageDistributionApiClient CreatePackageDistributionHttpClient(string? baseUrl, PackageCredentials? packageCredentials)
        {
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            return new PackageDistributionApiClient(httpClientFactory, baseUrl, packageCredentials);
        }

        public ISubscriptionApiClient CreateSubscriptionHttpClient(string? baseUrl, PackageCredentials? packageCredentials)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            return new SubscriptionApiClient(httpClientFactory, baseUrl, packageCredentials, mapper);
        }
    }
}
