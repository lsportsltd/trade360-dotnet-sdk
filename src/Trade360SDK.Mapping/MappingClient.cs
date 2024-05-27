using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common;
using Trade360SDK.Common.Models;
using Trade360SDK.Mapping.Entities;
using Trade360SDK.Mapping.Enums;
using Trade360SDK.Mapping.Models;

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

        public async Task<IEnumerable<Sport>> GetSportsAsync(CancellationToken cancellationToken)
        {
            var sportsCollection = await GetEntityAsync<SportsCollection>(
                "sports/get",
                new Request(),
                cancellationToken);
            return sportsCollection.Sports ?? Enumerable.Empty<Sport>();
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync(CancellationToken cancellationToken)
        {
            var locationsCollection = await GetEntityAsync<LocationsCollection>(
                "locations/get",
                new Request(),
                cancellationToken);
            return locationsCollection.Locations ?? Enumerable.Empty<Location>();
        }

        public async Task<IEnumerable<League>> GetLeaguesAsync(
            IEnumerable<int> sportIds,
            IEnumerable<int> locationIds,
            SubscriptionStatus subscriptionStatus,
            CancellationToken cancellationToken)
        {
            var leaguesCollection = await GetEntityAsync<LeaguesCollection>(
                "leagues/get",
                new LeaguesRequest(sportIds, locationIds, subscriptionStatus),
                cancellationToken);
            return leaguesCollection.Leagues ?? Enumerable.Empty<League>();
        }
    }
}
