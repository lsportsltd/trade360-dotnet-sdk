using AutoMapper;
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

        public SnapshotApiFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISnapshotInplayApiClient CreateInplayHttpClient(SnapshotApiSettings settings)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var httpClient = _serviceProvider.GetRequiredService<HttpClient>();
            return new InplaySnapshotApiClient(httpClient, settings, mapper);
        }

        public ISnapshotPrematchApiClient CreatePrematchHttpClient(SnapshotApiSettings settings)
        {
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var httpClient = _serviceProvider.GetRequiredService<HttpClient>();
            return new PrematchSnapshotApiClient(httpClient, settings, mapper);
        }
    }
}
