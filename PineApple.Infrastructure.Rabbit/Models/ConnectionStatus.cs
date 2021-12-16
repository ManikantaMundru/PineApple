using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Models
{
   public class ConnectionStatus
    {
        public ConnectionStatus(string connectionState, string message)
        {
            ConnectionState = connectionState;
            Message = message;
        }

        public string ConnectionState { get; set; }
        public string Message { get; set; }
    }
}
