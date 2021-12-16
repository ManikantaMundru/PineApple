using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PineApple.Infrastructure.Data.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        TRepository Repository<TRepository>();

        IDbTransaction Transaction { get; }

        void Commit();

        void Rollback();

    }
}
