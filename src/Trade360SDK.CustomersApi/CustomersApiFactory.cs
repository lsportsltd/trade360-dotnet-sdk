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

        public IMetadataHttpClient CreateMetadataHttpClient(string? baseUrl, PackageCredentials? packageCredentials)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            return new MetadataHttpClient(httpClientFactory, baseUrl, packageCredentials, mapper);
        }

        public IPackageDistributionHttpClient CreatePackageDistributionHttpClient(string? baseUrl, PackageCredentials? packageCredentials)
        {
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            return new PackageDistributionHttpClient(httpClientFactory, baseUrl, packageCredentials);
        }

        public ISubscriptionHttpClient CreateSubscriptionHttpClient(string? baseUrl, PackageCredentials? packageCredentials)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            return new SubscriptionHttpClient(httpClientFactory, baseUrl, packageCredentials, mapper);
        }

        public IPackageQueryHttpClient CreatePackageQueryHttpClient(string? baseUrl, PackageCredentials? packageCredentials)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            return new PackageQueryHttpClient(httpClientFactory, baseUrl, packageCredentials, mapper);
        }
    }
}
