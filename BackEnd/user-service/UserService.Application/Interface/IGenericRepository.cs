using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Service.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> expression);
        Task<(List<T>, int)> PagingData(Expression<Func<T, bool>> query, Expression<Func<T, object>> sort, Expression<Func<T, object>> include, string sortOrder = "asc", int skip = 0, int take = 10);
        void Create(T entity);
        void CreateRange(List<T> entity);
        void Update(T entity);
        void UpdateRange(List<T> entity);
        void Delete(T entity);
        void DeleteRange(List<T> entity);
    }
}
