using System.Configuration;
using RabbitMQ.Client;

var rmqHost = ConfigurationManager.AppSettings["rmqHost"];
var rmqUserName = ConfigurationManager.AppSettings["rmqUserName"];
var rmqPassword = ConfigurationManager.AppSettings["rmqPassword"];

ConnectionFactory connectionFactory = new()
{
    HostName = rmqHost,
    UserName = rmqUserName,
    Password = rmqPassword,
    VirtualHost = "StmInPlay",
    AutomaticRecoveryEnabled = true,
    RequestedHeartbeat = TimeSpan.FromSeconds(60),
    NetworkRecoveryInterval = TimeSpan.FromSeconds(1),
};

using var connection = connectionFactory.CreateConnection();

Console.WriteLine("connection status {0}", connection.IsOpen);

connection.Close();
