using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings.Contracts
{
   public interface ISubscriberExchangeSettings
    {
        List<string> RoutingKeys { get; set; }
    }
}
