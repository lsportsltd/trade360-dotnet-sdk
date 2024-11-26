using System;
using AutoMapper;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;
using Trade360SDK.CustomersApi.Http;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;

namespace Trade360SDK.CustomersApi
{
    public class SubscriptionApiClient : BaseHttpClient, ISubscriptionApiClient
    {
        private readonly IMapper _mapper;
        public SubscriptionApiClient(IHttpClientFactory httpClientFactory, string? baseUrl, PackageCredentials? packageCredentials, IMapper mapper)
            : base(httpClientFactory, baseUrl, packageCredentials)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(baseUrl ?? throw new ArgumentNullException(nameof(baseUrl)));
            _mapper = mapper;
        }
        
        public Task<PackageQuotaResponse> GetPackageQuotaAsync(CancellationToken cancellationToken)
            => PostEntityAsync<PackageQuotaResponse>("/package/GetPackageQuota",  cancellationToken);

        public async Task<FixtureScheduleCollectionResponse> GetInplayFixtureSchedule(GetFixtureScheduleRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetFixtureScheduleRequest>(requestDto);

            var response = await PostEntityAsync<FixtureScheduleCollectionResponse>(
                "Fixtures/InPlaySchedule",
                request,
                cancellationToken);
            return response;
        }

        public async Task<FixtureSubscriptionCollectionResponse> SubscribeByFixture(FixtureSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<FixtureSubscriptionRequest>(requestDto);

            var response = await PostEntityAsync<FixtureSubscriptionCollectionResponse>(
                "Fixtures/Subscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<FixtureSubscriptionCollectionResponse> UnSubscribeByFixture(FixtureSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<FixtureSubscriptionRequest>(requestDto);

            var response = await PostEntityAsync<FixtureSubscriptionCollectionResponse>(
                "Fixtures/UnSubscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<LeagueSubscriptionCollectionResponse> SubscribeByLeague(LeagueSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<LeagueSubscriptionRequest>(requestDto);

            var response = await PostEntityAsync<LeagueSubscriptionCollectionResponse>(
                "Leagues/Subscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<LeagueSubscriptionCollectionResponse> UnSubscribeByLeague(LeagueSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<LeagueSubscriptionRequest>(requestDto);

            var response = await PostEntityAsync<LeagueSubscriptionCollectionResponse>(
                "Leagues/UnSubscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<GetSubscriptionResponse> GetSubscriptions(GetSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetSubscriptionRequest>(requestDto);

            var response = await PostEntityAsync<GetSubscriptionResponse>(
                "Fixtures/Get",
                request,
                cancellationToken);
            return response;
        }

        public async Task<CompetitionSubscriptionCollectionResponse> SubscribeByCompetition(CompetitionSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<CompetitionSubscriptionRequest>(requestDto);

            var response = await PostEntityAsync<CompetitionSubscriptionCollectionResponse>(
                "Outright/Subscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<CompetitionSubscriptionCollectionResponse> UnSubscribeByCompetition(CompetitionSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<CompetitionSubscriptionRequest>(requestDto);

            var response = await PostEntityAsync<CompetitionSubscriptionCollectionResponse>(
                "Outright/UnSubscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<GetManualSuspensionResponse> GetAllManualSuspensions(CancellationToken cancellationToken)
        {
            var response = await PostEntityAsync<GetManualSuspensionResponse>(
                "Markets/ManualSuspension/GetAll",

                cancellationToken);
            return response;
        }

        public async Task<ChangeManualSuspensionResponse> AddManualSuspension(ChangeManualSuspensionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<ChangeManualSuspensionRequest>(requestDto);

            var response = await PostEntityAsync<ChangeManualSuspensionResponse>(
                "Markets/ManualSuspension/Activate",
                request,
                cancellationToken);
            return response;
        }

        public async Task<ChangeManualSuspensionResponse> RemoveManualSuspension(ChangeManualSuspensionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<ChangeManualSuspensionRequest>(requestDto);

            var response = await PostEntityAsync<ChangeManualSuspensionResponse>(
                "Markets/ManualSuspension/Deactivate ",
                request,
                cancellationToken);
            return response;
        }

        public async Task<GetFixtureMetadataCollectionResponse> GetFixtureMetadataAsync(GetFixtureMetadataRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetFixtureMetadataRequest>(requestDto);

            var response = await GetEntityAsync<GetFixtureMetadataCollectionResponse>(
                "Fixtures/GetSubscribedMetaData",
                request,
                cancellationToken);
            return response;
        }
    }
}
