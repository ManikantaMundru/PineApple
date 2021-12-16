using PineApple.Infrastructure.Rabbit.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings.Contracts
{
   public interface IExchangeSettings
    {
        List<string> RoutingKeys { get; set; }
        Dictionary<string, object> Arguments { get; set; }
        bool AutoDelete { get; set; }
        bool Durable { get; set; }
        string Name { get; set; }       
        ExchangeTypes Type { get; set; }
    }
}
