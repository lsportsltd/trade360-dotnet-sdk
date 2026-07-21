using System;
using System.Collections.Generic;
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
                throw new InvalidOperationException(
                    $"Unexpected Snapshot API response ({(int)httpResponse.StatusCode} {httpResponse.StatusCode}) from '{uri}'. " +
                    $"Expected Trade360 Header/Body envelope. Response: {Truncate(rawResponse, 500)}");
            }

            if (response.Body == null)
            {
                // Trade360 returns Body:null for empty result sets; treat as empty for collection responses.
                if (TryCreateEmptyEnumerable<TEntity>(out var empty))
                {
                    return empty;
                }

                throw new InvalidOperationException(
                    $"API returned null Body (no matching data or invalid filters). Header Type={response.Header.Type}, MsgGuid={response.Header.MsgGuid}.");
            }

            return response.Body;
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            {
                return value ?? string.Empty;
            }

            return value.Substring(0, maxLength) + "...";
        }

        private static bool TryCreateEmptyEnumerable<TEntity>(out TEntity empty) where TEntity : class
        {
            empty = null!;
            var type = typeof(TEntity);
            if (!type.IsGenericType)
            {
                return false;
            }

            var definition = type.GetGenericTypeDefinition();
            if (definition != typeof(IEnumerable<>) &&
                definition != typeof(IReadOnlyCollection<>) &&
                definition != typeof(ICollection<>) &&
                definition != typeof(IList<>) &&
                definition != typeof(List<>))
            {
                return false;
            }

            var elementType = type.GetGenericArguments()[0];
            empty = (TEntity)(object)Array.CreateInstance(elementType, 0);
            return true;
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
