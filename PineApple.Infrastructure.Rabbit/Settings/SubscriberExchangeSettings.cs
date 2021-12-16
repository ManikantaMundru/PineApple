﻿using RabbitMQ.Client;
using PineApple.Infrastructure.Rabbit.Models;
using PineApple.Infrastructure.Rabbit.Settings.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings
{
    public class SubscriberExchangeSettings : ISubscriberExchangeSettings
    {
        public SubscriberExchangeSettings()
        {
            RoutingKeys = new List<string>();
            Arguments = new Dictionary<string, object>();
        }
        public Dictionary<string, object> Arguments { get; set; }
        public List<string> RoutingKeys { get; set; }

        public bool AutoDelete { get; set; }

        public bool Durable { get; set; }

        public string Name { get; set; }
        public ExchangeTypes Type { get; set; }



    }
}
