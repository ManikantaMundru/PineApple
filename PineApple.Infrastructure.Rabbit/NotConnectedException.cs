using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Rabbit
{
   public class BrokerNotConnectedException: Exception
    {
        public BrokerNotConnectedException(): base("")
        {

        }
    }
}
