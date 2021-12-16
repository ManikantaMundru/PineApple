using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings.Contracts
{
   public interface ISubscriberSettings
    {
        QueueSettings Queue { get; set; }
        SubscriberExchangeSettings Exchange { get; set; }
        List<SubscriberExchangeSettings> FromExchanges { get; set; }
        List<MoniterQueueSettings> MoniterQueues { get; set; }
        ushort PrefetchCount { get; set; }
        ushort PrefetchSize { get; set; }
    }
}
