using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.Domain.Interfaces;

namespace Task.Domain.Repository
{
    public interface IBaseRepository<T, TId> : IAdd<T>, IEdit<T>, IDelete<Guid>, IListE<T, TId>, ITransaction
    {

    }
}
