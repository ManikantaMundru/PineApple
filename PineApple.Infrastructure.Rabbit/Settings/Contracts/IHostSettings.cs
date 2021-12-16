using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings.Contracts
{
    interface IHostSettings
    {
        string HostName { get; set; }
        string Password { get; set; }
        string UserName { get; set; }
        string VirtualHost { get; set; }
    }
}
