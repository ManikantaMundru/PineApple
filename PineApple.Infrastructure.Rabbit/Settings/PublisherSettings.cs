using PineApple.Infrastructure.Rabbit.Settings.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings
{
    public class PublisherSettings : IPublisherSettings
    {
        public ExchangeSettings Exchange { get; set; }
        public List<ExchangeSettings> ToExchange { get; set; }
        public bool BatchConfirm { get; set; }
        public int BatchConfirmSize { get; set; }

        public PublisherSettings()
        {
            ToExchange = new List<ExchangeSettings>();
        }
    }
}
