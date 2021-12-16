using PineApple.Infrastructure.Rabbit.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Subscribers
{
    public interface ISubscriberClient : IDisposable
    {
        void Connect(Action<MessageEvent> onReceive);
        void ExtraRuntimeBinding(List<string> routingkeys);
        void Disconnect();
    }
}
