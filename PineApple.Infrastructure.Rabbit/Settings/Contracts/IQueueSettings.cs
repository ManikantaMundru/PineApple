using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings.Contracts
{
   public interface IQueueSettings
    {
        bool AutoDelete { get; set; }
        int MessageTimeToLive { get; set; }
        bool Durable { get; set; }
        bool Exclusive { get; set; }
        bool AutoAck { get; set; }
        string Name { get; set; }
        int QueueTimeToLive { get; set; }
        bool FetchStatus { get; set; }
        string ConsumerTag { get; set; }
    }
}
