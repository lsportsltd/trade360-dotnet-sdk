﻿using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.SnapshotApi.Configuration;
using Trade360SDK.SnapshotApi.Entities.Requests;

namespace Trade360SDK.SnapshotApi.Http
{
    public abstract class BaseHttpClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        private readonly int _packageId;
        private readonly string _username;
        private readonly string _password;

        protected BaseHttpClient(HttpClient httpClient, SnapshotApiSettings settings)
        {
            _httpClient = httpClient;

            _packageId = settings.PackageId;
            _username = settings.Username;
            _password = settings.Password;
        }

        protected async Task<TEntity> PostEntityAsync<TEntity>(
            string uri,
            BaseRequest request, CancellationToken cancellationToken) where TEntity : class
        {
            request.PackageId = _packageId;
            request.UserName = _username;
            request.Password = _password;

            var requestJson = JsonConvert.SerializeObject(request);
            var content = new StringContent(
                requestJson,
                Encoding.UTF8,
                "application/json");
            var httpResponse = await _httpClient.PostAsync(uri, content, cancellationToken);

            var rawResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<BaseResponse<TEntity>>(rawResponse);

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

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}