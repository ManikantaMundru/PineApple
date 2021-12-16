using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using PineApple.Infrastructure.Rabbit.Communication.Contracts;
using PineApple.Infrastructure.Rabbit.Models;
using PineApple.Infrastructure.Rabbit.Settings.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Communication
{


    public class QueueListener : IQueueListener
    {
        private readonly IConnection _connection;
        private IModel _channel;
        private bool _disposed;
        private readonly ISubscriberSettings _settings;
        private Action<MessageEvent> _onReceive;

        public void Connect()
        {
            try
            {
                _channel = _connection.CreateModel();
                var argument = GetArguments(_settings.Queue.MessageTimeToLive, _settings.Queue.QueueTimeToLive);
                _channel.QueueDeclare(_settings.Queue.Name, _settings.Queue.Durable, _settings.Queue.Exclusive, _settings.Queue.AutoDelete, argument);

                _channel.ExchangeDeclare(_settings.Exchange.Name, _settings.Exchange.Type.ToName(), _settings.Exchange.Durable, _settings.Exchange.AutoDelete);
                foreach (var exchange in _settings.FromExchanges)
                {
                    _channel.ExchangeDeclare(exchange.Name, exchange.Type.ToName(), exchange.Durable, exchange.AutoDelete);
                    if (exchange.RoutingKeys?.Any() ?? false)
                    {
                        //logger
                        exchange.RoutingKeys.ForEach(
                            rk => _channel.ExchangeBind(_settings.Exchange.Name, exchange.Name, rk, exchange.Arguments));
                    }
                    else
                    {
                        //logger
                        _settings.Exchange.RoutingKeys.ForEach(
                            rk => _channel.ExchangeBind(_settings.Exchange.Name, exchange.Name, rk, exchange.Arguments));
                    }
                }
                //logger
                if (_settings.PrefetchSize > 0)
                {
                    _channel.BasicQos(_settings.PrefetchSize, _settings.PrefetchCount, false);
                }
                _settings.Exchange.RoutingKeys.ForEach(rk => _channel.QueueBind(_settings.Queue.Name, _settings.Exchange.Name, rk));
                if (_settings.Exchange.Type == ExchangeTypes.Headers)
                {
                    _channel.QueueBind(_settings.Queue.Name, _settings.Exchange.Name, "", _settings.Exchange.Arguments);
                }

            }
            catch (Exception ex)
            {

                //logger
            }
            if (!_settings.MoniterQueues.Any())
                return;
            //logger
            foreach (var monitorQueue in _settings.MoniterQueues)
            {
                try
                {
                    var monArgs = GetArguments(monitorQueue.MessageTimeToLive, 0);
                    _channel.QueueDeclare(monitorQueue.Name, monitorQueue.Durable, monitorQueue.Exclusive, false, monArgs);
                    monitorQueue.FromExchangeRoutingKey.ForEach(fromExchangeAndRoutingKey => fromExchangeAndRoutingKey.Value.ForEach(
                        rk => _channel.QueueBind(monitorQueue.Name, fromExchangeAndRoutingKey.Key, rk)));
                    //logger
                }
                catch (Exception ex)
                {

                    //logger
                }

            }
        }
        public QueueListener(ISubscriberSettings settings, IConnection connection)
        {
            _settings = settings;
            _connection = connection;
        }
        public void BindExtraOnRunTime(List<string> routingKeys)
        {
            try
            {
                routingKeys.ForEach(rk => _channel.QueueBind(_settings.Queue.Name, _settings.Exchange.Name, rk));
                //logger
            }
            catch (Exception ex)
            {
                //logger
                throw;
            }
        }
      
        private Dictionary<string, object> GetArguments(int messageTimeToLive, int queueTimeToiLive)
        {
            var arguments = new Dictionary<string, object>();
            if (messageTimeToLive > 0)
            {
                arguments.Add("x-message-ttl", messageTimeToLive);

            }
            if (queueTimeToiLive > 0)
            {
                arguments.Add("x-expire", queueTimeToiLive);
            }
            return arguments;
        }
        public void Dispose()
        {
            Dispose(true);
        }
        protected void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                if (_channel != null && _channel.IsClosed == false)
                {
                    _channel.Close();
                    _channel.Dispose();
                }
            }
            _disposed = true;
        }
        public void Connect(Action<MessageEvent> onReceive)
        {
            Connect();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (s, e) => Consumer_Received(_channel, s, e);
            var consumerTag = $"{Environment.MachineName}-{Assembly.GetEntryAssembly().GetName().Name}";
            _channel.BasicConsume(_settings.Queue.Name, false, $"{consumerTag}-{Guid.NewGuid()}", consumer);
            //logger
            _onReceive = onReceive;
        }
        public MessageEvent Pull()
        {
            var result = _channel.BasicGet(_settings.Queue.Name, _settings.Queue.AutoAck);
            if (result == null)

                return null;

            return new MessageEvent
            {
                Acknowledge = () => { if (_settings.Queue.AutoAck) return; _channel.BasicAck(result.DeliveryTag, false); },

                Reject = b => { if (_settings.Queue.AutoAck) { return; } _channel.BasicReject(result.DeliveryTag, b); },

                EventModel = new EventModel
                {
                    Body = Encoding.UTF8.GetString(result.Body),
                    Headers = result.BasicProperties.Headers,
                    DeliveryTag = result.DeliveryTag,
                    RoutingKey = result.RoutingKey,
                    QueueStatus = GetQueueStatus(),
                    ReceivedOnUtc = DateTime.UtcNow
                }
            };
        }
        private void Consumer_Received(IModel channel,object sender,BasicDeliverEventArgs e)
        {
            try
            {
                _onReceive(new MessageEvent
                {
                    Acknowledge = () => { if (_settings.Queue.AutoAck) return; channel.BasicAck(e.DeliveryTag, false); },
                    Reject = b => { if (_settings.Queue.AutoAck) return; channel.BasicReject(e.DeliveryTag, b); },
                    EventModel = new EventModel
                    {
                        Body = Encoding.UTF8.GetString(e.Body),
                        Headers = e.BasicProperties.Headers,
                        DeliveryTag = e.DeliveryTag,
                        RoutingKey = e.RoutingKey,
                        QueueStatus = GetQueueStatus(),
                        ReceivedOnUtc = DateTime.UtcNow
                    }
                });
            }
            catch (Exception ex)
            {
                //logger
                throw;
            }
            finally
            {
                if(_settings.Queue.AutoAck)
                {
                    channel.BasicAck(e.DeliveryTag, false);
                }
            }
        }
        private QueueStatus GetQueueStatus()
        {
            QueueStatus status = null;
            if(_settings.Queue.FetchStatus)
            {
                var arguments = GetArguments(_settings.Queue.MessageTimeToLive, _settings.Queue.QueueTimeToLive);
                var queueStatus = _channel.QueueDeclare(_settings.Queue.Name, _settings.Queue.Durable, _settings.Queue.Exclusive, _settings.Queue.AutoDelete, arguments);
                status = new QueueStatus
                {
                    ConsumerCount = queueStatus.ConsumerCount,
                    MessageCount = queueStatus.MessageCount,
                    QueueName = queueStatus.QueueName
                };
            }
            return status;
        }

    }
}
