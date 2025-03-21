﻿using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Configuration;
using Trade360SDK.SnapshotApi.Entities.Requests;

namespace Trade360SDK.SnapshotApi.Http
{
    public abstract class BaseHttpClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        private readonly int _packageId;
        private readonly string? _username;
        private readonly string? _password;

        protected BaseHttpClient(IHttpClientFactory httpClientFactory, Trade360Settings settings, PackageCredentials? packageCredentials)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(settings.SnapshotApiBaseUrl ?? throw new InvalidOperationException());
            _packageId = packageCredentials!.PackageId;
            _username = packageCredentials.Username;
            _password = packageCredentials.Password;
        }

        protected async Task<TEntity> PostEntityAsync<TEntity>(
            string uri,
            BaseRequest request, CancellationToken cancellationToken) where TEntity : class
        {
            request.PackageId = _packageId;
            request.UserName = _username;
            request.Password = _password;

            var content = SerializeRequest(request);
            var httpResponse = await _httpClient.PostAsync(uri, content, cancellationToken);

            var rawResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = DeserializeResponse<BaseResponse<TEntity>>(rawResponse);

            if (response == null || response.Header == null)
            {
                throw new InvalidOperationException("'Header' property is missed. Please, ensure that you use Trade360 url");
            }

            if (response.Body == null)
            {
                throw new InvalidOperationException("'Body' property is missed. Please, ensure that you use Trade360 url");
            }

            return response.Body;
        }

        private HttpContent SerializeRequest(BaseRequest request)
        {
            var requestJson = JsonSerializer.Serialize(request, request.GetType());
            return new StringContent(requestJson, Encoding.UTF8, "application/json");
        }

        private TEntity? DeserializeResponse<TEntity>(string rawResponse) where TEntity : class
        {
            return JsonSerializer.Deserialize<TEntity>(rawResponse);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
