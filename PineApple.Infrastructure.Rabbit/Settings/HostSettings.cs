using PineApple.Infrastructure.Rabbit.Settings.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Settings
{
    public class HostSettings : IHostSettings
    {
        public string HostName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string VirtualHost { get; set; }
    }
}
