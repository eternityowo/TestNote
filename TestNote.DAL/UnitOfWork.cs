using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using TestNote.DAL.Contracts;

namespace TestNote.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, IBaseRepository> _repositories;
        private readonly object _lockObject = new object();

        public INoteDBContext Context { get; private set; }

        public UnitOfWork(INoteDBContext context)
        {
            _repositories = new Dictionary<Type, IBaseRepository>();
            Context = context;
        }

        public IBaseRepository<T> GetRepository<T>()
        {
            // Check if repository exist in cache
            if (_repositories.ContainsKey(typeof(T)))
                return _repositories[typeof(T)] as IBaseRepository<T>;

            // If not then create a new instance and add to cache
            var repositoryType = typeof(BaseRepository<>).MakeGenericType(typeof(T));
            var repository = (IBaseRepository<T>)Activator.CreateInstance(repositoryType, Context);
            _repositories.Add(typeof(T), repository);

            return repository;
        }

        public void RollBack()
        {
            var changedEntries = Context.DbContext.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

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

        public int SaveChanges()
        {
            try
            {
                Monitor.Enter(_lockObject);
                Context.DbContext.SaveChanges();

                var entities = from e in ((NoteDBContext)Context).ChangeTracker.Entries()
                               where e.State == EntityState.Added
                                   || e.State == EntityState.Modified
                               select e.Entity;
                foreach (var entity in entities)
                {
                    var validationContext = new ValidationContext(entity);
                    Validator.ValidateObject(entity, validationContext);
                }

                return ((NoteDBContext)Context).SaveChanges();
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
    }
}
