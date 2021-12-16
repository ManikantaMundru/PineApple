using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Models
{
   public class QueueStatus: IQueueStatus
    {
       public uint ConsumerCount { get; internal set; }
       public uint MessageCount { get; internal set; }
       public string QueueName { get; internal set; }
    }
}
