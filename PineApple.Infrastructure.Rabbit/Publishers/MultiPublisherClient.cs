using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PineApple.Infrastructure.Rabbit.Communication;
using PineApple.Infrastructure.Rabbit.ConnectionHandlers;
using PineApple.Infrastructure.Rabbit.Models;
using PineApple.Infrastructure.Rabbit.Settings.Contracts;

namespace PineApple.Infrastructure.Rabbit.Publishers
{
    public class MultiPublisherClient : IPublisherClient
    {

        private bool _disconnected = true;
        private readonly IPublisherSettings _settings;
        private readonly IConnectionPoolHandler _connectionPoolHandler;
        private IList<ExchangePublisher> _publishers;

        public MultiPublisherClient(IPublisherSettings settings, IConnectionPoolHandler connectionPoolHandler)
        {
            _settings = settings;
            _connectionPoolHandler = connectionPoolHandler;
        }
        public void Connect()
        {
            _publishers = _connectionPoolHandler.Connections.Select(x => new ExchangePublisher(_settings, x)).ToList();
            _publishers.ForEach(p => p.Connect());
            _disconnected = false;
        }

        public void Disconnect()
        {
            foreach (var item in _publishers)
            {
                item?.Dispose();
            }
            _disconnected = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disconnected)
            {
                return;
            }
            if (disposing)
            {
                Disconnect();
            }
        }
        public PublishResult[] Publish<T>(T payload, string routingKey, IDictionary<string, object> headers = null) where T : class, new()
        {
            try
            {
                var payloadJson = payload.ToJson();
                if (_disconnected)
                {
                    throw new BrokerNotConnectedException();
                }

                var publisherHeaders = headers ?? new Dictionary<string, object> { { "eventType", routingKey } };
                if (!publisherHeaders.ContainsKey("node"))
                {
                    publisherHeaders.Add("node", Environment.MachineName);
                }

                var results = new ConcurrentBag<PublishResult>();

                var tasks = _publishers.Select(exchangePublisher => Task.Run(() =>
                 {
                     try
                     {
                         var result = exchangePublisher.Publish(routingKey, payloadJson, publisherHeaders);
                         results.Add(result);
                     }
                     catch (Exception ex)
                     {

                         throw ex;
                     }
                 }));

                Task.WaitAll(tasks.ToArray());

                return results.ToArray();
            }
            catch (Exception)
            {

                return new PublishResult[] { };
            }
        }
    }
}
