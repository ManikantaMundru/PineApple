using PineApple.Infrastructure.Rabbit.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Subscribers
{
   public interface IPullClient: IDisposable
    {
        MessageEvent Pull();
        void Disconnect();
    }
}
