using System;

namespace Trade360SDK.Feed.RabbitMQ.Exceptions
{
    public class RabbitMqFeedException : Exception
    {
        public RabbitMqFeedException(string message) : base(message) { }
        public RabbitMqFeedException(string message, Exception innerException) : base(message, innerException) { }
    }
}