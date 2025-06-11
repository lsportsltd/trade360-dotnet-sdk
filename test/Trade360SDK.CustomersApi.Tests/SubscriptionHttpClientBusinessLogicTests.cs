using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;
using Trade360SDK.CustomersApi.Mapper;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests
{
    public class SubscriptionHttpClientBusinessLogicTests
    {
        private readonly SubscriptionHttpClient _client;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly Mock<IMapper> _mockMapper;

        public SubscriptionHttpClientBusinessLogicTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockMapper = new Mock<IMapper>();
            var packageCredentials = new PackageCredentials { PackageId = 1, Username = "user", Password = "pass" };
            var httpClient = new HttpClient(_mockHttpMessageHandler.Object) { BaseAddress = new Uri("http://localhost") };
            var httpClientFactory = new TestHttpClientFactory(httpClient);
            _client = new SubscriptionHttpClient(httpClientFactory, "http://localhost", packageCredentials, _mockMapper.Object);
        }

        [Fact]
        public async Task GetPackageQuotaAsync_ShouldReturnExpectedResponse()
        {
            var expectedResponse = new PackageQuotaResponse { Quota = 100 };
            SetupHttpResponse(expectedResponse);
            var result = await _client.GetPackageQuotaAsync(CancellationToken.None);
            result.Should().NotBeNull();
            result.Quota.Should().Be(100);
        }

        [Fact]
        public async Task GetInplayFixtureSchedule_WithValidRequest_ShouldReturnExpectedResponse()
        {
            var expectedResponse = new FixtureScheduleCollectionResponse { Schedules = new List<FixtureSchedule> { new FixtureSchedule { FixtureId = 1 } } };
            var requestDto = new GetFixtureScheduleRequestDto { Fixtures = new List<int> { 1 } };
            var mappedRequest = new GetFixtureScheduleRequest();
            _mockMapper.Setup(x => x.Map<GetFixtureScheduleRequest>(requestDto)).Returns(mappedRequest);
            SetupHttpResponse(expectedResponse);
            var result = await _client.GetInplayFixtureSchedule(requestDto, CancellationToken.None);
            result.Should().NotBeNull();
            result.Schedules.Should().HaveCount(1);
            _mockMapper.Verify(x => x.Map<GetFixtureScheduleRequest>(requestDto), Times.Once);
        }

        [Fact]
        public async Task SubscribeByFixture_WithValidRequest_ShouldReturnExpectedResponse()
        {
            var expectedResponse = new FixtureSubscriptionCollectionResponse { Subscriptions = new List<FixtureSubscription> { new FixtureSubscription { FixtureId = 1 } } };
            var requestDto = new FixtureSubscriptionRequestDto { Fixtures = new List<int> { 1 } };
            var mappedRequest = new FixtureSubscriptionRequest();
            _mockMapper.Setup(x => x.Map<FixtureSubscriptionRequest>(requestDto)).Returns(mappedRequest);
            SetupHttpResponse(expectedResponse);
            var result = await _client.SubscribeByFixture(requestDto, CancellationToken.None);
            result.Should().NotBeNull();
            result.Subscriptions.Should().HaveCount(1);
            _mockMapper.Verify(x => x.Map<FixtureSubscriptionRequest>(requestDto), Times.Once);
        }

        [Fact]
        public async Task GetInplayFixtureSchedule_WithNullDto_ShouldThrowArgumentNullException()
        {
            Func<Task> act = async () => await _client.GetInplayFixtureSchedule(null!, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task SubscribeByFixture_WithNullDto_ShouldThrowArgumentNullException()
        {
            Func<Task> act = async () => await _client.SubscribeByFixture(null!, CancellationToken.None);
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetPackageQuotaAsync_WithErrorResponse_ShouldThrowTrade360Exception()
        {
            SetupHttpResponse(new PackageQuotaResponse(), HttpStatusCode.BadRequest, withError: true);
            Func<Task> act = async () => await _client.GetPackageQuotaAsync(CancellationToken.None);
            await act.Should().ThrowAsync<Trade360SDK.Common.Exceptions.Trade360Exception>();
        }

        private void SetupHttpResponse<T>(T responseObject, HttpStatusCode statusCode = HttpStatusCode.OK, bool withError = false) where T : class
        {
            var baseResponse = new Trade360SDK.CustomersApi.Entities.Base.BaseResponse<T>
            {
                Header = new Trade360SDK.CustomersApi.Entities.Base.HeaderResponse
                {
                    RequestId = Guid.NewGuid().ToString(),
                    Errors = withError ? new List<Trade360SDK.CustomersApi.Entities.Base.Error> { new Trade360SDK.CustomersApi.Entities.Base.Error { Message = "error" } } : new List<Trade360SDK.CustomersApi.Entities.Base.Error>()
                },
                Body = responseObject
            };
            var json = JsonSerializer.Serialize(baseResponse);
            var httpResponse = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(json, Encoding.UTF8)
            };
            httpResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);
        }

        private class TestHttpClientFactory : IHttpClientFactory
        {
            private readonly HttpClient _httpClient;
            public TestHttpClientFactory(HttpClient httpClient) => _httpClient = httpClient;
            public HttpClient CreateClient(string name) => _httpClient;
            public HttpClient CreateClient() => _httpClient;
        }
    }
}
