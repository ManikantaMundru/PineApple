using PineApple.Infrastructure.Rabbit.Settings.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings
{
    public class MoniterQueueSettings : IMoniterQueueSettings
    {
        public int MessageTimeToLive { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public string Name { get; set; }
        public bool AutoAck { get; set; }
        public Dictionary<string, List<string>> FromExchangeRoutingKey { get; set; }

        public MoniterQueueSettings()
        {
            AutoAck = false;
            MessageTimeToLive = 120000;
        }
    }
}
