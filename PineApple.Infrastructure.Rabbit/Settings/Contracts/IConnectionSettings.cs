using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings.Contracts
{
    public interface IConnectionSettings
    {
        List<HostSettings> HostSettings { get; set; }
        uint ReconnectSeconds { get; set; }

        bool AutoReconnect { get; set; }

        ushort Heartbeat { get; set; }

        int Port { get; set; }
    }
}
