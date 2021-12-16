using System;
using System.Runtime.Serialization;

namespace PineApple.Infrastructure.Rabbit.Publishers
{
    [Serializable]
    internal class BrokerNotConnectedException : Exception
    {
        public BrokerNotConnectedException()
        {
        }

        public BrokerNotConnectedException(string message) : base(message)
        {
        }

        public BrokerNotConnectedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BrokerNotConnectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}