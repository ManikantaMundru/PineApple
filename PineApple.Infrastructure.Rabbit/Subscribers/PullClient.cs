using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PineApple.Infrastructure.Rabbit.Communication;
using PineApple.Infrastructure.Rabbit.ConnectionHandlers;
using PineApple.Infrastructure.Rabbit.Models;
using PineApple.Infrastructure.Rabbit.Settings.Contracts;

namespace PineApple.Infrastructure.Rabbit.Subscribers
{
   public class PullClient: IPullClient
    {
        private bool _connected;
        private readonly ISubscriberSettings _settings;
        private readonly IConnectionPoolHandler _connectionPoolHanlder;
        private List<QueueListener> _listeners;

        public PullClient(ISubscriberSettings subscriberSettings, IConnectionPoolHandler connectionPoolHandler)
        {
            _settings = subscriberSettings;
            _connectionPoolHanlder = connectionPoolHandler;
            _connected = false;
        }

        public MessageEvent Pull()
        {
            if (_connected)
            {
               if(_connectionPoolHanlder.Connections.Count != 1)
                {
                    throw new NotSupportedException("");
                }

                _listeners = _connectionPoolHanlder.Connections.Select(c => new QueueListener(_settings, c)).ToList();
                _listeners.ForEach(c => c.Connect());
                _connected = true;
            }
            return _listeners[0].Pull();
        }

        public void Disconnect()
        {
            foreach (var listner in _listeners)
            {
                listner.Dispose();
            }
            _connected = false;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (_connected)
            {
                return;
            }

            if (disposing)
            {
                Disconnect();
            }

        }
    }
}
