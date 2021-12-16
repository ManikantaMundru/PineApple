using System;
using System.Collections.Generic;
using System.Text;

namespace PineApple.Infrastructure.Data.Contracts
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}
