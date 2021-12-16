using PineApple.Infrastructure.Rabbit.Settings.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings
{
    public class SubscriberSettings : ISubscriberSettings
    {
        public QueueSettings Queue { get; set; }
        public SubscriberExchangeSettings Exchange { get; set; }
        public List<SubscriberExchangeSettings> FromExchanges { get; set; }
        public List<MoniterQueueSettings> MoniterQueues { get; set; }
        public ushort PrefetchCount { get; set; }
        public ushort PrefetchSize { get; set; }

        public SubscriberSettings()
        {
            FromExchanges = new List<SubscriberExchangeSettings>();
            MoniterQueues = new List<MoniterQueueSettings>();
        }
    }
}
