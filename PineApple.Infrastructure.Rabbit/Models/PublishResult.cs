using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit.Models
{
   public class PublishResult
    {
        public string RoutingKey { get; set; }

        public string Body { get; set; }

        public string Connection { get; set; }

        public string Channel { get; set; }

        public IDictionary<string, object> Headers { get; set; }

        public TimeSpan PublishDuration { get; set; }

        public long PublishDurationMilliseconds { get; set; }

    }
}
