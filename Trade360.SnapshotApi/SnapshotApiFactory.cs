using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using Trade360SDK.SnapshotApi.Configuration;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.SnapshotApi
{
    public class SnapshotApiFactory : ISnapshotApiFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public SnapshotApiFactory(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public ISnapshotInplayApiClient CreateInplayHttpClient(SnapshotApiSettings settings)
        {
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var httpClient = _serviceProvider.GetRequiredService<HttpClient>();
            return new InplaySnapshotApiClient(httpClient, settings, mapper);
        }

        public ISnapshotPrematchApiClient CreatePrematchHttpClient(SnapshotApiSettings settings)
        {
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var httpClient = _serviceProvider.GetRequiredService<HttpClient>();
            return new PrematchSnapshotApiClient(httpClient, settings, mapper);
        }
    }
}
