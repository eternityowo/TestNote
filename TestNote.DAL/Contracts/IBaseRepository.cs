using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TestNote.DAL.Models;

namespace TestNote.DAL.Contracts
{
    public interface IBaseRepository { }

    public interface IBaseRepository<T> : IBaseRepository
    {
        IQueryable<T> All { get; }
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        T GetById(Guid id);
        IQueryable<T> Get(out int total, string orderBy, int skip, int take = 0,
            bool descOrder = true, IEnumerable<Expression<Func<T, bool>>> conditions = null);
        void Delete(Guid id);
        void DeleteRange(IEnumerable<T> entities);
        bool Exists(Guid id);
        List<NoteModel> Get(out int total, string v1, int v2, int maxValue, bool v3, Func<object, object> p);
    }
}
