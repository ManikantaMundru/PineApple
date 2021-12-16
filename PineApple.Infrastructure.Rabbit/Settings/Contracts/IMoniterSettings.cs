using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings.Contracts
{
   public interface IMoniterSettings
    {
        IQueueSettings Queue { get; set; }
        string ExchangeName { get; set; }
        List<string> RoutingKeys { get; set; }
    }
}
