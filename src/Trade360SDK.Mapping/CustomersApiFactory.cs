using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using Trade360SDK.Api.Abstraction;
using Trade360SDK.Api.Abstraction.Interfaces;

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
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var httpClient = _serviceProvider.GetRequiredService<HttpClient>();
            return new MetadataApiClient(httpClient, settings, mapper);
        }

        public IPackageDistributionApiClient CreatePackageDistributionHttpClient(CustomersApiSettings settings)
        {
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var httpClient = _serviceProvider.GetRequiredService<HttpClient>();
            return new PackageDistributionApiClient(httpClient, settings);
        }

        public ISubscriptionApiClient CreateSubscriptionHttpClient(CustomersApiSettings settings)
        {
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var httpClient = _serviceProvider.GetRequiredService<HttpClient>();
            return new SubscriptionApiClient(httpClient, settings, mapper);
        }
    }
}
