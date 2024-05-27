using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Common
{
    public abstract class BaseHttpClient
    {
        private readonly HttpClient _httpClient;

        private readonly int _packageId;
        private readonly string _username;
        private readonly string _password;

        public BaseHttpClient(string customerApi, int packageId, string username, string password)
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(customerApi) };

            _packageId = packageId;
            _username = username;
            _password = password;
        }

        public BaseHttpClient(HttpClient httpClient, int packageId, string username, string password)
        {
            _httpClient = httpClient;

            _packageId = packageId;
            _username = username;
            _password = password;
        }

        protected async Task<TEntity> GetEntityAsync<TEntity>(
            string uri,
            Request request, CancellationToken cancelationToken) where TEntity : class
        {
            request.PackageId = _packageId;
            request.UserName = _username;
            request.Password = _password;

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");
            var httpResponse = await _httpClient.PostAsync(uri, content, cancelationToken);
            httpResponse.EnsureSuccessStatusCode();
            var rawResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<Response<TEntity>>(rawResponse);
            if (response == null || response.Header == null)
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

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
