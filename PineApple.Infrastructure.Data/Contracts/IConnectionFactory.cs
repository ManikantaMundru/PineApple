using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PineApple.Infrastructure.Data.Contracts
{
   public interface IConnectionFactory
    {
        IDbConnection CreateOpenConnection();
    }
}
