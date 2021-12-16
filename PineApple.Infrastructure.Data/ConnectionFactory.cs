using PineApple.Infrastructure.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PineApple.Infrastructure.Data
{
    public class ConnectionFactory<TConnection> : IConnectionFactory
          where TConnection : IDbConnection, new()
    {
        private readonly string _connectionString;

        public ConnectionFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _connectionString = connectionString;
        }

        public IDbConnection CreateOpenConnection()
        {
            var conn = new TConnection { ConnectionString = _connectionString };

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
            }
            catch (Exception exception)
            {
                throw new Exception("An error occured while connecting to the database. See innerException for details.", exception);
            }

            return conn;
        }
    }
}
