using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common;
using Trade360SDK.Common.Models;
using Trade360SDK.Mapping.Entities;

namespace Trade360SDK.Mapping
{
    public class MappingClient : BaseHttpClient
    {
        public MappingClient(string customerApi, int packageId, string username, string password)
            : base(customerApi, packageId, username, password)
        {
        }

        public MappingClient(HttpClient httpClient, int packageId, string username, string password)
            : base(httpClient, packageId, username, password)
        {
        }

        public async Task<IEnumerable<Sport>> GetSports(CancellationToken cancellationToken)
        {
            var sportsCollection = await GetEntityAsync<SportsCollection>("sports/get", new Request(), cancellationToken);
            return sportsCollection.Sports ?? Enumerable.Empty<Sport>();
        }
    }
}
