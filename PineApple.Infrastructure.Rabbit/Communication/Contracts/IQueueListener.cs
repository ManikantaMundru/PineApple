using PineApple.Infrastructure.Rabbit.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Communication.Contracts
{
   public interface IQueueListener
    {
        void Connect(Action<MessageEvent> onReceive);
        void BindExtraOnRunTime(List<string> routingKeys);
        void Dispose();
    }
}
