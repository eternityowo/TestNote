using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestNote.DAL.Contracts
{
    public interface IUnitOfWork
    {
        IBaseRepository<T> GetRepository<T>() where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
