using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TestNote.DAL.Contracts;
using TestNote.DAL.Entities;
using TestNote.DAL.Extensions;
using TestNote.DAL.Models;

namespace TestNote.DAL
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
           where TEntity : class
    {
        protected INoteDBContext Context { get; private set; }
        protected DbSet<TEntity> DbSet { get; set; }

        public IQueryable<TEntity> All
        {
            get { return DbSet; }
        }

        public BaseRepository(INoteDBContext dbContext)
        {
            if (dbContext != null)
            {
                Context = dbContext;
                DbSet = Context.DbContext.Set<TEntity>();
            }
            else
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
        }


        public IQueryable<TEntity> AllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = All;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public TEntity GetById(Guid id)
        {
            return DbSet.Find(id);
        }

        public IQueryable<TEntity> Get(out int total, string orderBy, int skip, int take = 0,
            bool descOrder = true, IEnumerable<Expression<Func<TEntity, bool>>> conditions = null)
        {
            var query = All;

            if (conditions != null)
            {
                foreach (var condition in conditions)
                {
                    query = query.Where(condition);
                }
            }

            query = query.OrderByField(orderBy, descOrder);
            total = query.Count();
            query = query.Skip(skip);
            if (take != 0)
            {
                query = query.Take(take);
            }
            return query;
        }

        public virtual void Add(TEntity entity)
        {
            var baseEntity = entity as BaseEntity;
            if (baseEntity != null && baseEntity.Id == Guid.Empty)
            {
                baseEntity.Id = Guid.NewGuid();
            }

            var dbEntityEntry = Context.DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
        }

        public virtual void Update(TEntity entity)
        {
            var dbEntityEntry = Context.DbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            var dbEntityEntry = Context.DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public bool Exists(Guid id)
        {           
            return DbSet.Find(id) != null;
        }

        public virtual void Delete(Guid id)
        {
            var entity = GetById(id);
            if (entity == null)
                return;

            Delete(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public List<NoteModel> Get(out int total, string v1, int v2, int maxValue, bool v3, Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}
