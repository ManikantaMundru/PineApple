using PineApple.Infrastructure.Rabbit.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Publishers
{
    public interface IPublisherClient
    {
        void Connect();
        PublishResult[] Publish<T>(T payload, string routingKey, IDictionary<string, object> headers = null) where T : class, new();
        void Disconnect();
    }
}
