using PineApple.Infrastructure.Rabbit.Settings.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings
{
    public class QueueSettings : IQueueSettings
    {
        public QueueSettings()
        {
            MessageTimeToLive = 7200000;
            AutoAck = true;
        }
        public bool AutoDelete { get; set; }
        public int MessageTimeToLive { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoAck { get; set; }
        public string Name { get; set; }
        public int QueueTimeToLive { get; set; }
        public bool FetchStatus { get; set; }
        public string ConsumerTag { get; set; }
    }
}
