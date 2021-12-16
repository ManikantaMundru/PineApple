using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using PineApple.Infrastructure.Rabbit.Models;
using PineApple.Infrastructure.Rabbit.Settings.Contracts;

namespace PineApple.Infrastructure.Rabbit.ConnectionHandlers
{
    public class ConnectionPoolHandler : IConnectionPoolHandler
    {

        private readonly IConnectionSettings _settings;
        private Dictionary<string, IConnection> _connections = new Dictionary<string, IConnection>();
        private bool _disposed;

       public event Action<ConnectionStatus> OnConnectionBlocked;
       public event Action<ConnectionStatus> OnConnectionShutdown;
       public event Action<string> OnConnectionOpened;
       public event Action<string> OnConnectionClosed;


        public ConnectionPoolHandler(IConnectionSettings settings)
        {
            _settings = settings;
        }


        private bool IsConnectionOpen()
        {
            var result = _connections.Any(c => c.Value.IsConnected());
            return result;
        }

        public void Connect()
        {
            if (IsConnectionOpen())
            {
                return;
            }

            _connections = _settings.HostSettings.ToDictionary(k => $"{k.HostName}.{k.VirtualHost}", host =>
                {
                    try
                    {
                        var factory = new ConnectionFactory
                        {
                            VirtualHost= host.VirtualHost,
                            UserName=host.UserName,
                            Password=host.Password,
                            HostName=host.HostName,
                            Port=_settings.Port,
                            RequestedHeartbeat= _settings.Heartbeat,
                            AutomaticRecoveryEnabled=_settings.AutoReconnect
                        };
                        factory.NetworkRecoveryInterval = 
                        TimeSpan.FromSeconds(_settings.ReconnectSeconds != 0 ? _settings.ReconnectSeconds : 2);

                        var connection = factory.CreateConnection();
                        connection.ConnectionShutdown += HandleOnConnectionShutdown;
                        connection.ConnectionBlocked += HandleOnConnectionBlocked;
                        connection.ConnectionUnblocked += HandleOnConnectionUnBlocked;
                        return connection;
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                });
        }

        private void HandleOnConnectionUnBlocked(object sender, EventArgs e)
        {
            OnConnectionOpened?.Invoke(ConnectionStatus());
        }

        private void HandleOnConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            OnConnectionShutdown?.Invoke(new ConnectionStatus(ConnectionStatus(), e.Cause.ToString()));
        }

        private void HandleOnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            OnConnectionBlocked?.Invoke(new ConnectionStatus(ConnectionStatus(), e.Reason));
        }


        private string ConnectionStatus() => string.Join("; ", _connections.Select(kvp => $"{kvp.Key}: connected: {kvp.Value.IsConnected()}"));


        public List<IConnection> Connections
        {
            get
            {
                if (IsConnectionOpen())
                {
                    return _connections.Values.ToList();
                }

                throw new InvalidOperationException("");
            }
        }

        public void dispose()
        {
            if (_disposed)
            {
                return;
            }
            Disconnect();
            _disposed = true;
        }
        public void Disconnect()
        {
            _connections.ForEach (connection =>
            {
                if (connection.Value.IsConnected())
                {
                    connection.Value.ConnectionShutdown -= HandleOnConnectionShutdown;
                    connection.Value.ConnectionUnblocked -= HandleOnConnectionUnBlocked;
                    connection.Value.ConnectionBlocked -= HandleOnConnectionBlocked;
                    connection.Value.Close();
                    connection.Value.Dispose();
                    OnConnectionClosed?.Invoke($"Connection Disposed:: {connection.Key}");
                }
            }) ;
        }
    }
}
