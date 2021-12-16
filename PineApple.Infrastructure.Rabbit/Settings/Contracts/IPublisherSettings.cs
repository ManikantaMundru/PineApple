using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings.Contracts
{
   public interface IPublisherSettings
    {
        ExchangeSettings Exchange { get; set; }
        List<ExchangeSettings> ToExchange { get; set; }
        bool BatchConfirm { get; set; }
        int BatchConfirmSize { get; set; }

    }
}
