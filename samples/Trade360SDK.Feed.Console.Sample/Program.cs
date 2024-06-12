using System.Configuration;
using Trade360SDK.Common.Enums;
using Trade360SDK.Feed;
using Trade360SDK.Feed.Console.Sample;

var rmqHost = "frizzy-magenta-pronghorn.in.rmq4.cloudamqp.com";
var username = ConfigurationManager.AppSettings["username"];
var password = ConfigurationManager.AppSettings["password"];
var packageIdValue = ConfigurationManager.AppSettings["packageId"] ;

if (username == null || password == null || !int.TryParse(packageIdValue, out int packageId))
{
    return;
}

using var feed = new Feed(rmqHost, username, password,
    packageId, PackageType.InPlay,
    100, TimeSpan.FromSeconds(60),
    new ConsoleLogger());

feed.Start();

Console.WriteLine("Click any key to stop message consumption");
Console.ReadLine();

feed.Stop();
