using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Models
{
    public class EventModel : IEventModel
    {
        public EventModel()
        {

        }

        public string Body { get; internal set; }
        public string RoutingKey { get; internal set; }

        public ulong DeliveryTag { get; internal set; }

        public IDictionary<string, object> Headers { get; internal set; }

        public IQueueStatus QueueStatus { get; internal set; }

        public DateTime ReceivedOnUtc { get; internal  set; }
    }

}
