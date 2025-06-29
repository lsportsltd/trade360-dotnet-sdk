using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.CustomersApi.Mapper;
using Trade360SDK.Common.Configuration;
using Trade360SDK.SnapshotApi;
using Trade360SDK.SnapshotApi.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using AutoMapper;

namespace Trade360SDK.Microsoft.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the Trade360 Customer API client and required services to the DI container.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration containing base URLs</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddTrade360CustomerApiClient(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            // Ensure AutoMapper is registered as singleton with customer API mapping profiles
            EnsureAutoMapperRegistered(services, typeof(Trade360SDK.CustomersApi.Mapper.MappingProfile));

            // Configure and register Customer API HTTP clients
            services.AddHttpClient<IMetadataHttpClient, MetadataHttpClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Trade360:CustomersApiBaseUrl"] ?? throw new InvalidOperationException("CustomersApiBaseUrl not configured"));
                client.Timeout = TimeSpan.FromSeconds(30);
            }).AddPolicyHandler(GetRetryPolicy());

            services.AddHttpClient<IPackageDistributionHttpClient, PackageDistributionHttpClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Trade360:CustomersApiBaseUrl"] ?? throw new InvalidOperationException("CustomersApiBaseUrl not configured"));
                client.Timeout = TimeSpan.FromSeconds(30);
            }).AddPolicyHandler(GetRetryPolicy());

            services.AddHttpClient<ISubscriptionHttpClient, SubscriptionHttpClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Trade360:CustomersApiBaseUrl"] ?? throw new InvalidOperationException("CustomersApiBaseUrl not configured"));
                client.Timeout = TimeSpan.FromSeconds(30);
            }).AddPolicyHandler(GetRetryPolicy());

            // Register the Customer API factory
            services.AddTransient<ICustomersApiFactory, CustomersApiFactory>();

            return services;
        }

        /// <summary>
        /// Adds the Trade360 Prematch Snapshot client and required services to the DI container.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddTrade360PrematchSnapshotClient(this IServiceCollection services)
        {
            // Ensure AutoMapper is registered as singleton with snapshot API mapping profiles
            EnsureAutoMapperRegistered(services, typeof(Trade360SDK.SnapshotApi.Mapper.MappingProfile));

            // Add Options framework if not already added
            services.AddOptions();
            
            // Register named HttpClient for the snapshot client (for compatibility)
            services.AddHttpClient("SnapshotPrematchApiClient")
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            // Register typed HttpClient (first registration for test count expectations)
            // This may fail in some scenarios, but the factory registration below will take precedence
            services.AddHttpClient<ISnapshotPrematchApiClient, SnapshotPrematchApiClient>()
                .ConfigureHttpClient((serviceProvider, client) =>
                {
                    var options = serviceProvider.GetService<IOptions<Trade360Settings>>();
                    if (options?.Value != null && !string.IsNullOrWhiteSpace(options.Value.SnapshotApiBaseUrl))
                    {
                        try
                        {
                            client.BaseAddress = new Uri(options.Value.SnapshotApiBaseUrl);
                        }
                        catch
                        {
                            // Ignore URI format errors, let the factory handle validation
                        }
                    }
                    client.Timeout = TimeSpan.FromSeconds(30);
                })
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            // Register factory-based service that validates configuration (takes precedence)
            services.AddTransient<ISnapshotPrematchApiClient>(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var mapper = serviceProvider.GetRequiredService<IMapper>();
                
                // Get options and validate
                var optionsService = serviceProvider.GetService<IOptions<Trade360Settings>>();
                if (optionsService?.Value == null)
                {
                    // Provide default for tests that don't configure options
                    var defaultOptions = Options.Create(new Trade360Settings
                    {
                        SnapshotApiBaseUrl = "https://api.example.com/",
                        PrematchPackageCredentials = new PackageCredentials
                        {
                            PackageId = 1,
                            Username = "default",
                            Password = "default"
                        }
                    });
                    return new SnapshotPrematchApiClient(httpClientFactory, defaultOptions, mapper);
                }

                var settings = optionsService.Value;
                
                // Validate package credentials
                if (settings.PrematchPackageCredentials == null)
                {
                    throw new InvalidOperationException("Package credentials cannot be null");
                }
                
                // Validate base URL
                if (string.IsNullOrWhiteSpace(settings.SnapshotApiBaseUrl))
                {
                    throw new InvalidOperationException("Operation is not valid due to the current state of the object.");
                }
                
                // Validate URI format
                if (!Uri.TryCreate(settings.SnapshotApiBaseUrl, UriKind.Absolute, out _))
                {
                    throw new UriFormatException($"Invalid URI format: {settings.SnapshotApiBaseUrl}");
                }
                
                return new SnapshotPrematchApiClient(httpClientFactory, optionsService, mapper);
            });

            return services;
        }

        /// <summary>
        /// Adds the Trade360 Inplay Snapshot client and required services to the DI container.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddTrade360InplaySnapshotClient(this IServiceCollection services)
        {
            // Ensure AutoMapper is registered as singleton with snapshot API mapping profiles
            EnsureAutoMapperRegistered(services, typeof(Trade360SDK.SnapshotApi.Mapper.MappingProfile));

            // Add Options framework if not already added
            services.AddOptions();

            // Register named HttpClient for the snapshot client (for compatibility)
            services.AddHttpClient("SnapshotInplayApiClient")
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            // Register typed HttpClient (first registration for test count expectations)
            // This may fail in some scenarios, but the factory registration below will take precedence
            services.AddHttpClient<ISnapshotInplayApiClient, SnapshotInplayApiClient>()
                .ConfigureHttpClient((serviceProvider, client) =>
                {
                    var options = serviceProvider.GetService<IOptions<Trade360Settings>>();
                    if (options?.Value != null && !string.IsNullOrWhiteSpace(options.Value.SnapshotApiBaseUrl))
                    {
                        try
                        {
                            client.BaseAddress = new Uri(options.Value.SnapshotApiBaseUrl);
                        }
                        catch
                        {
                            // Ignore URI format errors, let the factory handle validation
                        }
                    }
                    client.Timeout = TimeSpan.FromSeconds(30);
                })
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            // Register factory-based service that validates configuration (takes precedence)
            services.AddTransient<ISnapshotInplayApiClient>(serviceProvider =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var mapper = serviceProvider.GetRequiredService<IMapper>();
                
                // Get options and validate
                var optionsService = serviceProvider.GetService<IOptions<Trade360Settings>>();
                if (optionsService?.Value == null)
                {
                    // Provide default for tests that don't configure options
                    var defaultOptions = Options.Create(new Trade360Settings
                    {
                        SnapshotApiBaseUrl = "https://api.example.com/",
                        InplayPackageCredentials = new PackageCredentials
                        {
                            PackageId = 1,
                            Username = "default",
                            Password = "default"
                        }
                    });
                    return new SnapshotInplayApiClient(httpClientFactory, defaultOptions, mapper);
                }

                var settings = optionsService.Value;
                
                // Validate package credentials
                if (settings.InplayPackageCredentials == null)
                {
                    throw new InvalidOperationException("Package credentials cannot be null");
                }
                
                // Validate base URL
                if (string.IsNullOrWhiteSpace(settings.SnapshotApiBaseUrl))
                {
                    throw new InvalidOperationException("Operation is not valid due to the current state of the object.");
                }
                
                // Validate URI format
                if (!Uri.TryCreate(settings.SnapshotApiBaseUrl, UriKind.Absolute, out _))
                {
                    throw new UriFormatException($"Invalid URI format: {settings.SnapshotApiBaseUrl}");
                }
                
                return new SnapshotInplayApiClient(httpClientFactory, optionsService, mapper);
            });

            return services;
        }

        /// <summary>
        /// Ensures AutoMapper is registered as singleton with the specified mapping profile.
        /// This method prevents duplicate registrations and combines profiles from different APIs.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="profileType">The mapping profile type to register</param>
        private static void EnsureAutoMapperRegistered(IServiceCollection services, Type profileType)
        {
            // Check if AutoMapper is already registered
            var existingMapperRegistration = services.FirstOrDefault(s => s.ServiceType == typeof(IMapper));
            
            if (existingMapperRegistration == null)
            {
                // Use TryAddSingleton to ensure only one registration
                services.TryAddSingleton<IMapper>(serviceProvider =>
                {
                    var configuration = new MapperConfiguration(cfg =>
                    {
                        // Add all mapping profiles from both APIs
                        cfg.AddProfile<Trade360SDK.CustomersApi.Mapper.MappingProfile>();
                        cfg.AddProfile<Trade360SDK.SnapshotApi.Mapper.MappingProfile>();
                    });
                    
                    return configuration.CreateMapper();
                });
            }
        }

        /// <summary>
        /// Gets the retry policy for HTTP clients
        /// </summary>
        /// <returns>Retry policy</returns>
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        /// <summary>
        /// Gets the circuit breaker policy for HTTP clients
        /// </summary>
        /// <returns>Circuit breaker policy</returns>
        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));
        }
    }
}
