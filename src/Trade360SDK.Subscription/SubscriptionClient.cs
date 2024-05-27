using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Subscription.Entities;
using Trade360SDK.Subscription.Models;

namespace Trade360SDK.Subscription
{
    public class SubscriptionClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly Request _request;

        public SubscriptionClient(string url, int packageId, string username, string password)
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(url) };
            _request = new Request(packageId, username, password);
        }

        public SubscriptionClient(HttpClient httpClient, int packageId, string username, string password)
        {
            _httpClient = httpClient;
            _request = new Request(packageId, username, password);
        }

        public async Task<PackageQuota> GetPackageQuotaAsync(CancellationToken cancelationToken)
        {
            var content = new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync("/package/GetPackageQuota", content, cancelationToken);
            httpResponse.EnsureSuccessStatusCode();
            var rawResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<Response<PackageQuota>>(rawResponse);
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
