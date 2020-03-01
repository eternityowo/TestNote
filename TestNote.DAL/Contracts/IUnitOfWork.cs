using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestNote.DAL.Contracts
{
    public interface IUnitOfWork
    {
        IBaseRepository<T> GetRepository<T>();
        void RollBack();
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
