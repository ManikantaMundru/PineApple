using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Models
{

   public delegate void MessageReject(bool requeue);
   public class MessageEvent
    {
        public IEventModel EventModel { get; set; }
        public Action Acknowledge { get; set; }
        public MessageReject Reject { get; set; }
    }
}
