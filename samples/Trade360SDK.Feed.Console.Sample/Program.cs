﻿using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.Common.Enums;
using Trade360SDK.Feed.Console.Sample;
using Trade360SDK.Feed.Console.Sample.Handlers;
using Trade360SDK.Feed.RabbitMQ;

var customersApi = "https://stm-api.qa.lsports.cloud";
var rmqHost = "frizzy-magenta-pronghorn.in.rmq4.cloudamqp.com";
var username = ConfigurationManager.AppSettings["username"];
var password = ConfigurationManager.AppSettings["password"];
var packageIdValue = ConfigurationManager.AppSettings["packageId"] ;

if (username == null || password == null || !int.TryParse(packageIdValue, out int packageId))
{
    return;
}

var serviceProvider = new ServiceCollection()
    .AddHttpClient()
    .BuildServiceProvider();

// HttpClientFactory optimizes connection pool usage (especially improtant for other SDKs that actively uses HTTP requests)
var httpCLientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
var httpCLient = httpCLientFactory.CreateClient();
httpCLient.BaseAddress = new Uri(customersApi);

using var rmqFeed = new RabbitMQFeed(
    httpCLient,
    rmqHost, username, password,
    packageId, PackageType.InPlay,
    100, TimeSpan.FromSeconds(60),
    new ConsoleLogger());

rmqFeed.AddEntityHandler(new HeartbeatHandler());

CancellationTokenSource cts = new CancellationTokenSource();

await rmqFeed.StartAsync(cts.Token);

Console.WriteLine("Click any key to stop message consumption");
Console.ReadLine();

await rmqFeed.StopAsync(cts.Token);
