using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;
using Trade360SDK.CustomersApi.Http;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.CustomersApi.Mapper;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;

namespace Trade360SDK.CustomersApi
{
    public class SubscriptionHttpClient : BaseHttpClient, ISubscriptionHttpClient
    {
        public SubscriptionHttpClient(IHttpClientFactory httpClientFactory, string? baseUrl, PackageCredentials? packageCredentials)
            : base(httpClientFactory, baseUrl, packageCredentials)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(baseUrl ?? throw new ArgumentNullException(nameof(baseUrl)));
        }
        
        public Task<PackageQuotaResponse> GetPackageQuotaAsync(CancellationToken cancellationToken)
            => PostEntityAsync<PackageQuotaResponse>("/package/GetPackageQuota",  cancellationToken);

        public async Task<FixtureScheduleCollectionResponse> GetInplayFixtureSchedule(GetFixtureScheduleRequestDto requestDto, CancellationToken cancellationToken)
        {
            if (requestDto == null) { throw new ArgumentNullException(nameof(requestDto)); }
            var request = CustomersApiMapper.Map(requestDto);

            var response = await PostEntityAsync<FixtureScheduleCollectionResponse>(
                "Fixtures/InPlaySchedule",
                request,
                cancellationToken);
            return response;
        }

        public async Task<FixtureSubscriptionCollectionResponse> SubscribeByFixture(FixtureSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            if (requestDto == null) throw new ArgumentNullException(nameof(requestDto));
            var request = CustomersApiMapper.Map(requestDto);

            var response = await PostEntityAsync<FixtureSubscriptionCollectionResponse>(
                "Fixtures/Subscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<FixtureSubscriptionCollectionResponse> UnSubscribeByFixture(FixtureSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = CustomersApiMapper.Map(requestDto);

            var response = await PostEntityAsync<FixtureSubscriptionCollectionResponse>(
                "Fixtures/UnSubscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<LeagueSubscriptionCollectionResponse> SubscribeByLeague(LeagueSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = CustomersApiMapper.Map(requestDto);

            var response = await PostEntityAsync<LeagueSubscriptionCollectionResponse>(
                "Leagues/Subscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<LeagueSubscriptionCollectionResponse> UnSubscribeByLeague(LeagueSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = CustomersApiMapper.Map(requestDto);

            var response = await PostEntityAsync<LeagueSubscriptionCollectionResponse>(
                "Leagues/UnSubscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<GetSubscriptionResponse> GetSubscriptions(GetSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = CustomersApiMapper.Map(requestDto);

            var response = await PostEntityAsync<GetSubscriptionResponse>(
                "Fixtures/Get",
                request,
                cancellationToken);
            return response;
        }

        public async Task<CompetitionSubscriptionCollectionResponse> SubscribeByCompetition(CompetitionSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = CustomersApiMapper.Map(requestDto);

            var response = await PostEntityAsync<CompetitionSubscriptionCollectionResponse>(
                "Outright/Subscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<CompetitionSubscriptionCollectionResponse> UnSubscribeByCompetition(CompetitionSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = CustomersApiMapper.Map(requestDto);

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
            var request = CustomersApiMapper.Map(requestDto);

            var response = await PostEntityAsync<ChangeManualSuspensionResponse>(
                "Markets/ManualSuspension/Activate",
                request,
                cancellationToken);
            return response;
        }

        public async Task<ChangeManualSuspensionResponse> RemoveManualSuspension(ChangeManualSuspensionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = CustomersApiMapper.Map(requestDto);

            var response = await PostEntityAsync<ChangeManualSuspensionResponse>(
                "Markets/ManualSuspension/Deactivate ",
                request,
                cancellationToken);
            return response;
        }

        public async Task<GetFixtureMetadataCollectionResponse> GetFixtureMetadataAsync(GetFixtureMetadataRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = CustomersApiMapper.Map(requestDto);

            var response = await GetEntityAsync<GetFixtureMetadataCollectionResponse>(
                "Fixtures/GetSubscribedMetaData",
                request,
                cancellationToken);
            return response;
        }
    }
}
