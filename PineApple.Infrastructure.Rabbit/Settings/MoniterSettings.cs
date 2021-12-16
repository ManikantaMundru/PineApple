using PineApple.Infrastructure.Rabbit.Settings.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings
{
    public class MoniterSettings : IMoniterSettings
    {
        public IQueueSettings Queue { get; set; }
        public string ExchangeName { get; set; }
        public List<string> RoutingKeys { get; set; }
    }
}
