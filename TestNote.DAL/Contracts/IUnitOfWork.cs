using System;
using System.Collections.Generic;
using System.Text;

namespace TestNote.DAL.Contracts
{
    public interface IUnitOfWork
    {
        INoteDBContext Context { get; }
        IBaseRepository<T> GetRepository<T>();
        void RollBack();
        int SaveChanges();
    }
}
