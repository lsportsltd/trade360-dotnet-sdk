using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Trade360SDK.Api.Abstraction
{
    public abstract class BaseHttpClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        private readonly int _packageId;
        private readonly string _username;
        private readonly string _password;

        protected BaseHttpClient(HttpClient httpClient, CustomersApiSettings settings)
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

            if (!httpResponse.IsSuccessStatusCode)
            {
                var rawErrorResponse = await httpResponse.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<BaseResponse<TEntity>>(rawErrorResponse);

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
            var response = JsonConvert.DeserializeObject<BaseResponse<TEntity>>(rawResponse);

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
                var errorResponse = JsonConvert.DeserializeObject<BaseResponse<TEntity>>(rawErrorResponse);

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
            var response = JsonConvert.DeserializeObject<BaseResponse<TEntity>>(rawResponse);

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
            var properties = request.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var queryString = new StringBuilder();

            foreach (var prop in properties)
            {
                var value = prop.GetValue(request);
                if (value == null) continue;
                if (value is System.Collections.IEnumerable enumerable && !(value is string))
                {
                    foreach (var item in enumerable)
                    {
                        queryString.Append($"{Uri.EscapeDataString(prop.Name)}={Uri.EscapeDataString(item.ToString())}&");
                    }
                }
                else
                {
                    queryString.Append($"{Uri.EscapeDataString(prop.Name)}={Uri.EscapeDataString(value.ToString())}&");
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
