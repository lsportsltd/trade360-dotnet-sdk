using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Exceptions;
using Trade360SDK.CustomersApi.Entities.Base;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Trade360SDK.CustomersApi.Http
{
    public abstract class BaseHttpClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        private readonly int _packageId;
        private readonly string? _username;
        private readonly string? _password;

        protected BaseHttpClient(IHttpClientFactory httpClientFactory, string? baseUrl, PackageCredentials? settings)
        {
            if (httpClientFactory == null)
                throw new ArgumentNullException(nameof(httpClientFactory));
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("Base URL cannot be null or empty.", nameof(baseUrl));
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(baseUrl);
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

            var requestJson = JsonSerializer.Serialize(request, request.GetType());
            var content = new StringContent(
                requestJson,
                Encoding.UTF8,
                "application/json");
            var httpResponse = await _httpClient.PostAsync(uri, content, cancellationToken);

            return await HandleResponse<TEntity>(httpResponse);
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

            return await HandleResponse<TEntity>(httpResponse);
        }

        private async Task<TEntity> HandleResponse<TEntity>(HttpResponseMessage httpResponse) where TEntity : class
        {
            if (httpResponse.IsSuccessStatusCode)
            {
                var rawResponse = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<BaseResponse<TEntity>>(rawResponse);

                if (response?.Header == null)
                {
                    throw new InvalidOperationException("'Header' property is missing. Please, ensure that you use the correct URL.");
                }

                if (response.Body == null)
                {
                    throw new InvalidOperationException("'Body' property is missing. Please, ensure that you use the correct URL.");
                }

                return response.Body;
            }
            else
            {
                var rawErrorResponse = await httpResponse.Content.ReadAsStringAsync();
                var errorResponse = JsonSerializer.Deserialize<BaseResponse<TEntity>>(rawErrorResponse);

                if (errorResponse?.Header?.Errors != null)
                {
                    var errors = errorResponse.Header.Errors.Select(x => x.Message);
                    throw new Trade360Exception("API call failed", errors);
                }

                // Handle specific status codes if needed
                switch (httpResponse.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        // Handle bad request
                        throw new Trade360Exception("Bad Request", rawErrorResponse);
                    case HttpStatusCode.Unauthorized:
                        // Handle unauthorized
                        throw new Trade360Exception("Unauthorized", rawErrorResponse);
                    case HttpStatusCode.Forbidden:
                        // Handle forbidden
                        throw new Trade360Exception("Forbidden", rawErrorResponse);
                    case HttpStatusCode.NotFound:
                        // Handle not found
                        throw new Trade360Exception("Not Found", rawErrorResponse);
                    case HttpStatusCode.InternalServerError:
                        // Handle server error
                        throw new Trade360Exception("Internal Server Error", rawErrorResponse);
                    default:
                        // Handle other status codes
                        throw new Trade360Exception(httpResponse.StatusCode.ToString(), rawErrorResponse);
                }
            }
        }

        private string BuildQueryString(object request)
        {
            if (request == null)
                return string.Empty;

            var json = JsonSerializer.Serialize(request, request.GetType());
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            var queryString = new StringBuilder();

            if (dictionary != null)
            {
                foreach (var kvp in dictionary)
                {
                    if (kvp.Value == null)
                        continue;

                    if (kvp.Value is JsonElement jsonElement)
                    {
                        if (jsonElement.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var element in jsonElement.EnumerateArray())
                            {
                                var elementValue = element.ToString();
                                if (!string.IsNullOrEmpty(elementValue))
                                {
                                    queryString.Append($"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(elementValue)}&");
                                }
                            }
                        }
                        else if (jsonElement.ValueKind != JsonValueKind.Null)
                        {
                            var elementValue = jsonElement.ToString();
                            if (!string.IsNullOrEmpty(elementValue))
                            {
                                queryString.Append($"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(elementValue)}&");
                            }
                        }
                    }
                    else
                    {
                        var valueString = kvp.Value.ToString();
                        if (!string.IsNullOrEmpty(valueString))
                        {
                            queryString.Append($"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(valueString)}&");
                        }
                    }
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
