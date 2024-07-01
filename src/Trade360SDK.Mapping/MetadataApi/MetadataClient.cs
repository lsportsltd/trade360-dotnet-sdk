using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common;
using Microsoft.Extensions.Options;
using Trade360SDK.Api.Common.Models.Requests.Base;
using Trade360SDK.Metadata;
using Trade360SDK.CustomersApi.MetadataApi.Requests;
using Trade360SDK.Common.Metadata.Requests;
using Trade360SDK.Common.Metadata.Responses;
using AutoMapper;

namespace Trade360SDK.CustomersApi.MetadataApi
{
    public class MetadataClient : BaseHttpClient, IMetadataClient
    {
        private readonly IMapper _mapper;
        public MetadataClient(HttpClient httpClient, IOptions<Trade360ApiSettings> options, IMapper mapper)
            : base(httpClient, options.Value)
        {
            httpClient.BaseAddress = new System.Uri(options.Value.BaseUrl);
            _mapper = mapper;
        }

        public async Task<IEnumerable<Sport>> GetSportsAsync(CancellationToken cancellationToken)
        {
            var sportsCollection = await GetEntityAsync<SportsCollectionResponse>(
                "sports/get",
                new BaseRequest(),
                cancellationToken);
            return sportsCollection.Sports ?? Enumerable.Empty<Sport>();
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync(CancellationToken cancellationToken)
        {
            var locationsCollection = await GetEntityAsync<LocationsCollection>(
                "locations/get",
                new BaseRequest(),
                cancellationToken);
            return locationsCollection.Locations ?? Enumerable.Empty<Location>();
        }

        public async Task<IEnumerable<League>> GetLeaguesAsync(GetLeaguesRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetLeaguesRequest>(requestDto);
            var leaguesCollection = await GetEntityAsync<LeaguesCollectionResponse>(
                "leagues/get",
                request,
                cancellationToken);
            return leaguesCollection.Leagues ?? Enumerable.Empty<League>();
        }

        public async Task<IEnumerable<Market>> GetMarketsAsync(GetMarketsRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetMarketsRequest>(requestDto);
            var leaguesCollection = await GetEntityAsync<MarketsCollectionResponse>(
                "markets/get",
                request, 
                cancellationToken);
            return leaguesCollection.Markets ?? Enumerable.Empty<Market>();
        }
    }
}
