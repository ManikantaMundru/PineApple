using PineApple.Infrastructure.Rabbit.Models;
using PineApple.Infrastructure.Rabbit.Settings.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings
{
    public class ExchangeSettings : IExchangeSettings
    {
        public List<string> RoutingKeys { get; set; }
        public Dictionary<string, object> Arguments { get; set; }
        public bool AutoDelete { get; set; }
        public bool Durable { get; set; }
        public string Name { get; set; }
        public ExchangeTypes Type { get; set; }

        public ExchangeSettings()
        {
            RoutingKeys = new List<string>();
            Arguments = new Dictionary<string, object>();
        }
    }
}
