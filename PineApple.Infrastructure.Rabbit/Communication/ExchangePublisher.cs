using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using PineApple.Infrastructure.Rabbit.Communication.Contracts;
using PineApple.Infrastructure.Rabbit.Models;
using PineApple.Infrastructure.Rabbit.Settings.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Communication
{
   internal sealed class ExchangePublisher : IExchangePublisher
    {
        private readonly IConnection _connection;
        private readonly IPublisherSettings _publisherSettings;
        private IModel _channel;
        private bool _disposed;
        //private readonly ILogger _logger = LogManager.GetCurrentLogger;
        private int _counter;

        public ExchangePublisher(IPublisherSettings publisherSettings, IConnection connection)
        {
            _connection = connection;
            _publisherSettings = publisherSettings;

        }

        public void Connect()
        {
            try
            {
                _channel = _connection.CreateModel();
                if(_publisherSettings.Exchange.Type== ExchangeTypes.Transient)
                {
                    return;
                }
                //_logger.Info("Channel binding to exchange ::")
                _channel.ExchangeDeclare(_publisherSettings.Exchange.Name,
                    _publisherSettings.Exchange.Type.ToName(),
                    _publisherSettings.Exchange.Durable,
                    _publisherSettings.Exchange.AutoDelete,
                    _publisherSettings.Exchange.Arguments
                    );
                foreach (var toExchange in _publisherSettings.ToExchange)
                {
                    _channel.ExchangeDeclare(toExchange.Name, toExchange.Type.ToName(), toExchange.Durable, toExchange.AutoDelete);

                    if(toExchange.RoutingKeys?.Any() ?? false)
                    {
                        //_logger.Info("Channel binding to exchange ::")
                        toExchange.RoutingKeys.ForEach(rk => _channel.ExchangeBind(toExchange.Name, _publisherSettings.Exchange.Name, rk, toExchange.Arguments));

                    }
                    else
                    {
                        //_logger.Info("Channel binding to exchange ::")
                        _publisherSettings.Exchange.RoutingKeys.ForEach(rk => _channel.ExchangeBind(toExchange.Name, _publisherSettings.Exchange.Name, rk, toExchange.Arguments));
                    }

                    _channel.BasicReturn += Channel_BasicReturn;
                    _channel.ConfirmSelect();
                }
            }
            catch (Exception ex)
            {

                //_logger.Error(ex,$"[Channel] :: could not create a channel . Error: {ex.Message}")
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _channel.BasicReturn -= Channel_BasicReturn;
            _channel.Close();
            _channel.Dispose();
            _disposed = true;
        }

        public PublishResult Publish(string routingKey, string body, IDictionary<string, object> headers)
        {
            var sw =Stopwatch.StartNew();
            _channel.BasicPublish(_publisherSettings.Exchange.Name, 
                routingKey, true,
                new BasicProperties
                {
                    Headers = headers
                },
                Encoding.UTF8.GetBytes(body));
            WaitForConfirms();
            sw.Stop();

            return new PublishResult
            {
                Body = body,
                Channel = _channel.ToString(),
                Connection = _connection.ToString(),
                PublishDuration = sw.Elapsed,
                PublishDurationMilliseconds = sw.ElapsedMilliseconds,
                Headers = headers,
                RoutingKey = routingKey
            };
        }

        private void Channel_BasicReturn(object sender, RabbitMQ.Client.Events.BasicReturnEventArgs e)
        {
            //_logger.warn($"[Channel]:: Publish failed with routing key '{e.RoutingKey} to exchange '{_settings.Exchange.Name}'");
        }

        public void WaitForConfirms()
        {
            if(!_publisherSettings.BatchConfirm || _publisherSettings.BatchConfirmSize <= 1)
            {
                _channel.WaitForConfirms(TimeSpan.FromSeconds(2));
            }
            else
            {
                _counter++;
                if(_counter % _publisherSettings.BatchConfirmSize == 0)
                {
                    _channel.WaitForConfirms(TimeSpan.FromSeconds(2));
                    _counter = 0;
                }
            }
        }

    }

}
