using System.Configuration;
using Trade360SDK.Feed;

var rmqHost = "frizzy-magenta-pronghorn.in.rmq4.cloudamqp.com";
var username = ConfigurationManager.AppSettings["username"];
var password = ConfigurationManager.AppSettings["password"];
var packageId = ConfigurationManager.AppSettings["packageId"] ;

using var feed = new Feed(rmqHost, username, password, int.Parse(packageId), 100, TimeSpan.FromSeconds(60));

feed.Start();
await Task.Delay(TimeSpan.FromMinutes(2));
feed.Stop();
