using PineApple.Infrastructure.Rabbit.Settings.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings
{
    public class ConnectionSettings : IConnectionSettings
    {
        //This has been changed
        public List<HostSettings> HostSettings { get; set; }
        public uint ReconnectSeconds { get; set; }
        public bool AutoReconnect { get; set; }
        public ushort Heartbeat { get; set; }
        public int Port { get; set; }

        public ConnectionSettings()
        {
            HostSettings = new List<HostSettings>();
            AutoReconnect = true;
            Heartbeat = 20;
            Port = 5672;
            ReconnectSeconds = 1;
        }
    }
}
