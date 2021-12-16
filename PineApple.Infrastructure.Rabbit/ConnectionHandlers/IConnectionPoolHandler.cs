using RabbitMQ.Client;
using PineApple.Infrastructure.Rabbit.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.ConnectionHandlers
{
    public interface IConnectionPoolHandler
    {
        void Connect();
        void Disconnect();
        event Action<ConnectionStatus> OnConnectionBlocked;
        event Action<ConnectionStatus> OnConnectionShutdown;
        event Action<string> OnConnectionOpened;
        event Action<string> OnConnectionClosed;
        List<IConnection> Connections { get; }
    }
}
