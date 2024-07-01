using System;

namespace Trade360SDK.Feed.RabbitMQ.Exceptions
{
    public class RabbitMQFeedException : Exception
    {
        public RabbitMQFeedException(string message) : base(message) { }
        public RabbitMQFeedException(string message, Exception innerException) : base(message, innerException) { }
    }
}