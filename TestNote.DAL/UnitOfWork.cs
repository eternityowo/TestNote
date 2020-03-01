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
        private readonly object _lockObject = new object();

        public UnitOfWork(DbContext context)
        {
            _repositories = new Dictionary<Type, IBaseRepository>();
            _context = context;
        }

        public IBaseRepository<T> GetRepository<T>()
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

        public void RollBack()
        {
            var changedEntries = _context.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Modified))
            {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Added))
            {
                entry.State = EntityState.Detached;
            }

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Deleted))
            {
                entry.State = EntityState.Unchanged;
            }
        }
        public Task<int> SaveChangesAsync()
        {
            var entities = ((NoteDBContext)_context).ChangeTracker
                .Entries()
                .Where(_ => _.State == EntityState.Added ||
                            _.State == EntityState.Modified);

            var errors = new List<ValidationResult>(); // all errors are here
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.TryValidateObject(entity, validationContext, errors, validateAllProperties: true);
            }

            return _context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            try
            {
                Monitor.Enter(_lockObject);

                var entities = _context.ChangeTracker
                    .Entries()
                    .Where(_ => _.State == EntityState.Added ||
                                _.State == EntityState.Modified);

                var errors = new List<ValidationResult>(); // all errors are here
                foreach (var entity in entities)
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.TryValidateObject(entity, validationContext, errors, validateAllProperties: true);
                }

                return _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException("Entity update failed - errors follow: " + ex.GetBaseException().Message, ex);
            }
            finally
            {
                Monitor.Exit(_lockObject);
            }
        }

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
