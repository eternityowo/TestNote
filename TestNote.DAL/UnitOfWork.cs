using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestNote.DAL.Contracts;

namespace TestNote.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DbContext _context;
        private readonly IDictionary<Type, IBaseRepository> _repositories;

        public UnitOfWork(DbContext context)
        {
            _repositories = new Dictionary<Type, IBaseRepository>();
            _context = context;
        }

        public IBaseRepository<T> GetRepository<T>() where T : class
        {
            //Check if repository exist in cache
            if (_repositories.ContainsKey(typeof(T)))
                return _repositories[typeof(T)] as IBaseRepository<T>;

            // If not then create a new instance and add to cache
            var repositoryType = typeof(BaseRepository<>).MakeGenericType(typeof(T));
            var repository = (IBaseRepository<T>)Activator.CreateInstance(repositoryType, _context);
            _repositories.Add(typeof(T), repository);

            return repository;
        }

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
        public int SaveChanges() => _context.SaveChanges();


        #region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_context != null)
                    {
                        _context.Dispose();
                    }
                }
            }
            _disposed = true;
        }

        #endregion
    }
}
