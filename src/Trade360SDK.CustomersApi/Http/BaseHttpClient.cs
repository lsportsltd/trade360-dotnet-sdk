﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.CustomersApi.Configuration;
using Trade360SDK.CustomersApi.Entities;
using Trade360SDK.CustomersApi.Entities.Base;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Trade360SDK.CustomersApi.Http
{
    public abstract class BaseHttpClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        private readonly int _packageId;
        private readonly string _username;
        private readonly string _password;

        protected BaseHttpClient(IHttpClientFactory httpClientFactory, CustomersApiSettings settings)
        {
            _httpClient = httpClientFactory.CreateClient();

            _packageId = settings.PackageId;
            _username = settings.Username;
            _password = settings.Password;
        }

        protected async Task<TEntity> PostEntityAsync<TEntity>(string uri, CancellationToken cancellationToken) where TEntity : class
        {
            return await PostEntityAsync<TEntity>(uri, new BaseRequest(), cancellationToken);
        }

        protected async Task<TEntity> PostEntityAsync<TEntity>(
            string uri,
            BaseRequest request, CancellationToken cancellationToken) where TEntity : class
        {
            request.PackageId = _packageId;
            request.UserName = _username;
            request.Password = _password;

            var requestJson = JsonSerializer.Serialize(request);
            var content = new StringContent(
                requestJson,
                Encoding.UTF8,
                "application/json");
            var httpResponse = await _httpClient.PostAsync(uri, content, cancellationToken);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var rawErrorResponse = await httpResponse.Content.ReadAsStringAsync();
                var errorResponse = JsonSerializer.Deserialize<BaseResponse<TEntity>>(rawErrorResponse);

                if (errorResponse is { Header: { } })
                {
                    var errors = errorResponse.Header.Errors?.Select(x => x.Message);
                    throw new Trade360Exception(errors);
                }

                httpResponse.EnsureSuccessStatusCode();
            }

            var rawResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<BaseResponse<TEntity>>(rawResponse);

            if (response?.Header == null)
            {
                throw new InvalidOperationException("'Header' property is missed. Please, ensure that you use Trade360 url");
            }

            if (response.Header.HttpStatusCode != HttpStatusCode.OK)
            {
                var errors = response.Header.Errors?.Select(x => x.Message);
                throw new Trade360Exception(errors);
            }

            if (response.Body == null)
            {
                throw new InvalidOperationException("'Body' property is missed. Please, ensure that you use Trade360 url");
            }

            return response.Body;
        }

        protected async Task<TEntity> GetEntityAsync<TEntity>(
            string uri, CancellationToken cancellationToken) where TEntity : class
        {
            return await GetEntityAsync<TEntity>(uri, new BaseRequest(), cancellationToken);
        }

        protected async Task<TEntity> GetEntityAsync<TEntity>(
          string uri, BaseRequest request, CancellationToken cancellationToken) where TEntity : class
        {
            // Assign common parameters to the request object
            request.PackageId = _packageId;
            request.UserName = _username;
            request.Password = _password;

            // Build the query string from the request object properties
            var queryString = BuildQueryString(request);
            var fullUri = $"{uri}?{queryString}";

            var httpResponse = await _httpClient.GetAsync(fullUri, cancellationToken);

            if (!httpResponse.IsSuccessStatusCode)
            {
                var rawErrorResponse = await httpResponse.Content.ReadAsStringAsync();
                var errorResponse = JsonSerializer.Deserialize<BaseResponse<TEntity>>(rawErrorResponse);

                if (errorResponse is { Header: { } })
                {
                    var errors = errorResponse.Header.Errors?.Select(x => x.Message);
                    throw new Trade360Exception(errors);
                }
                else
                {
                    httpResponse.EnsureSuccessStatusCode();
                }
            }

            var rawResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<BaseResponse<TEntity>>(rawResponse);

            if (response?.Header == null)
            {
                throw new InvalidOperationException("'Header' property is missed. Please, ensure that you use Trade360 url");
            }

            if (response.Header.HttpStatusCode != HttpStatusCode.OK)
            {
                var errors = response.Header.Errors?.Select(x => x.Message);
                throw new Trade360Exception(errors);
            }

            if (response.Body == null)
            {
                throw new InvalidOperationException("'Body' property is missed. Please, ensure that you use Trade360 url");
            }

            return response.Body;
        }

        private string BuildQueryString(object request)
        {
            var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            var queryString = new StringBuilder();

            if (dictionary != null)
                foreach (var kvp in dictionary)
                {
                    if (kvp.Value is JsonElement jsonElement)
                    {
                        if (jsonElement.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var element in jsonElement.EnumerateArray())
                            {
                                queryString.Append(
                                    $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(element.ToString())}&");
                            }
                        }
                        else
                        {
                            queryString.Append(
                                $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(jsonElement.ToString())}&");
                        }
                    }
                    else
                    {
                        queryString.Append(
                            $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value.ToString())}&");
                    }
                }

            // Remove the trailing '&'
            if (queryString.Length > 0)
            {
                queryString.Length--;
            }

            return queryString.ToString();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
