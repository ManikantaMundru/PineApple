using PineApple.Infrastructure.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Data
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IConnectionFactory _connectionFactory;

        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkFactory(IConnectionFactory connectionFactory, IServiceProvider serviceProvider)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_connectionFactory, _serviceProvider);
        }
    }
}
