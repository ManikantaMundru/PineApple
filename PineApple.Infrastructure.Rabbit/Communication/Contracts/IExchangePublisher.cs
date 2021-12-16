using PineApple.Infrastructure.Rabbit.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Communication.Contracts
{
    public interface IExchangePublisher
    {
        void Connect();
        PublishResult Publish(string routingKey, string body, IDictionary<string, object> headers);

        void Dispose();
    }
}
