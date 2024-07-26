using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using Trade360SDK.CustomersApi.Configuration;
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

        public IMetadataApiClient CreateMetadataHttpClient(CustomersApiSettings settings)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            return new MetadataApiClient(httpClientFactory, settings, mapper);
        }

        public IPackageDistributionApiClient CreatePackageDistributionHttpClient(CustomersApiSettings settings)
        {
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            return new PackageDistributionApiClient(httpClientFactory, settings);
        }

        public ISubscriptionApiClient CreateSubscriptionHttpClient(CustomersApiSettings settings)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            return new SubscriptionApiClient(httpClientFactory, settings, mapper);
        }
    }
}
