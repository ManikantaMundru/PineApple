using PineApple.Infrastructure.Data.Contracts;
using System;
using System.Data;

namespace PineApple.Infrastructure.Data
{
    public abstract class RepositoryBase : IDisposable
    {
        private bool _disposed;

        private bool _unitOfWork;

        private readonly IConnectionFactory _connectionFactory;

        private IDbConnection _connection;

        protected IDbTransaction Transaction { get; private set; }

        protected IDbConnection Connection
        {
            get
            {
                if (_connection == null && !_disposed)
                    _connection = _connectionFactory.CreateOpenConnection();

                return _connection;
            }
        }

        protected RepositoryBase(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        internal void SetUnitOfWork(IDbConnection connection, IDbTransaction transaction)
        {
            if (_connection != null)
                throw new InvalidOperationException("This instance can not be associated with a unit-of-work scope because it already belongs to another scope.");

            _unitOfWork = true;

            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            Transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        }

        public void Dispose()
        {
            _disposed = true;

            if (_unitOfWork)
                return;

            // No support for transaction outside of a unit-of-work, but release transaction for in case.

            try
            {
                // Also does rollback if not committed
                Transaction?.Dispose();
            }
            catch { }

            try
            {
                // Also closes connection
                _connection?.Dispose();
            }
            catch { }

            Transaction = null;
            _connection = null;
        }
    }
}