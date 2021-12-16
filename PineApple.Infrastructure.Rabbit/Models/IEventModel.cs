using System;
using System.Collections.Generic;

namespace PineApple.Infrastructure.Rabbit.Models
{
    public interface IEventModel
    {
        string Body { get; }
        ulong DeliveryTag { get; }
        string RoutingKey { get; }
        IDictionary<string, object> Headers { get; }
        IQueueStatus QueueStatus { get; }
        DateTime ReceivedOnUtc { get; }
    }
}