using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings.Contracts
{
    public interface IMoniterQueueSettings
    {
        int MessageTimeToLive { get; set; } 
        bool Durable { get; set; }
        bool Exclusive { get; set; }
        string Name { get; set; }

        bool AutoAck { get; set; }
        Dictionary<string, List<string>> FromExchangeRoutingKey { get; set; }
    }
}
