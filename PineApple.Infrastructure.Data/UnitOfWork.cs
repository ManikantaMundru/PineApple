using Microsoft.Extensions.DependencyInjection;
using PineApple.Infrastructure.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace PineApple.Infrastructure.Data
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceScope _serviceScope;

        public IDbTransaction Transaction { get; private set; }

        public IDbConnection Connection { get; private set; }

        public UnitOfWork(IConnectionFactory connectionFactory, IServiceProvider serviceProvider)
        {
            if (connectionFactory == null)
                throw new ArgumentNullException(nameof(connectionFactory));

            Connection = connectionFactory.CreateOpenConnection();
            Transaction = Connection.BeginTransaction();

            if (Transaction == null)
                throw new IOException("Unable to start Transaction.");

            _serviceScope = serviceProvider.CreateScope();
        }

        /// <summary>
        /// Gets the implementation for the specified repository contract from the dependency injection container.
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <returns>The <seealso cref="DapperRepositoryBase"/> based implementation for the requested interface.</returns>
        public TRepository Repository<TRepository>()
        {
            var repo = _serviceScope.ServiceProvider.GetRequiredService<TRepository>();

            if (!(repo is DapperRepositoryBase dapperRepo))
                throw new ArgumentException($"The implementation of requested repository '{typeof(TRepository).FullName}' must extend from base type '{typeof(DapperRepositoryBase).FullName}'.");

            dapperRepo.SetUnitOfWork(Connection, Transaction);

            return repo;
        }

        public void Commit()
        {
            var conn = Transaction?.Connection ?? Connection;

            try
            {
                Transaction?.Commit();
            }
            catch
            {
                Transaction?.Rollback();
                throw;
            }
            finally
            {
                Transaction?.Dispose();
                Transaction = null;
                conn?.Close();
                conn?.Dispose();
            }
        }

        public void Rollback()
        {
            try
            {
                Transaction?.Rollback();
                Transaction?.Connection?.Close();
            }
            finally
            {
                Transaction?.Connection?.Dispose();
                Transaction?.Dispose();
                Transaction = null;
            }
        }

        public void Dispose()
        {
            try
            {
                _serviceScope.Dispose();
            }
            catch { }

            try
            {
                // Also does rollback if not committed
                Transaction?.Dispose();
            }
            catch { }

            try
            {
                // Also closes connection
                Connection?.Dispose();
            }
            catch { }

            Transaction = null;
            Connection = null;
        }
    }
}
