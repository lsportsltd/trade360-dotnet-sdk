using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using Trade360SDK.Api.Abstraction.Interfaces;
using Trade360SDK.Api.Subscription;
using Trade360SDK.Common;
using Trade360SDK.Metadata;

namespace Trade360SDK.CustomersApi
{
    public class CustomersApiFactory : ICustomersApiFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public CustomersApiFactory(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
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
            return new PackageDistributionApiClient(httpClient, settings, mapper);
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
