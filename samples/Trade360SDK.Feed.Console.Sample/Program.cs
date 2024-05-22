using System.Configuration;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var rmqHost = "frizzy-magenta-pronghorn.in.rmq4.cloudamqp.com";
var username = ConfigurationManager.AppSettings["username"];
var password = ConfigurationManager.AppSettings["password"];
var packageId = ConfigurationManager.AppSettings["packageId"] ;

ConnectionFactory connectionFactory = new()
{
    HostName = rmqHost,
    Port = 5672,
    UserName = username,
    Password = password,
    VirtualHost = "StmInPlay",
    AutomaticRecoveryEnabled = true,
    RequestedHeartbeat = TimeSpan.FromSeconds(60),
    NetworkRecoveryInterval = TimeSpan.FromSeconds(1),
    DispatchConsumersAsync = true,
};

using var connection = connectionFactory.CreateConnection();
if (!connection.IsOpen)
{
    Console.WriteLine("Failed to connect");
    return;
}

using var channel = connection.CreateModel();

AsyncEventingBasicConsumer consumer = new(channel);
consumer.Received += async (ch, ea) =>
{
    var bytes = ea.Body.ToArray();
    var rawMessage = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
    Console.WriteLine(rawMessage);
};
var consumerTag = channel.BasicConsume(
    queue: $"_{packageId}_",
    autoAck: true,
    consumer: consumer);

Console.WriteLine("Click any key to stop message consumption");
Console.ReadLine();

channel.BasicCancel(consumerTag);
connection.Close();
