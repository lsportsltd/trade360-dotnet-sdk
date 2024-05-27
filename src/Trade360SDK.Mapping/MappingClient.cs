using System;
using System.Net.Http;

namespace Trade360SDK.Mapping
{
    public class MappingClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly int _packageId;
        private readonly string _username;
        private readonly string _password;

        public MappingClient(string customerApi, int packageId, string username, string password)
        {
            _httpClient = new HttpClient() { BaseAddress = new Uri(customerApi) };

            _packageId = packageId;
            _username = username;
            _password = password;
        }

        public MappingClient(HttpClient httpClient, int packageId, string username, string password)
        {
            _httpClient = httpClient;

            _packageId = packageId;
            _username = username;
            _password = password;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
