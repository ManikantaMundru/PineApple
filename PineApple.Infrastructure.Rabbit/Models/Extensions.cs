using System;
using RabbitMQ.Client;

namespace PineApple.Infrastructure.Rabbit.Models
{
    public static class Extensions
    {
        public static string ToName(this ExchangeTypes exchangeType)
        {
            switch (exchangeType)
            {
                case ExchangeTypes.Fanout:
                    return ExchangeType.Fanout;
                case ExchangeTypes.Topic:
                    return ExchangeType.Topic;
                case ExchangeTypes.Direct:
                    return ExchangeType.Direct;
                case ExchangeTypes.Headers:
                    return ExchangeType.Headers;
                case ExchangeTypes.Transient:
                    return string.Empty;
                default:
                    throw new ArgumentException($"Unknown exchange type: {exchangeType}");
            }
        }
    }
}
