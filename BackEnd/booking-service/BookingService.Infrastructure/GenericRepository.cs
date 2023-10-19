using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookingService.Service.Interface;

namespace BookingService.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected BookingDbContext _context { get; set; }

        public GenericRepository(BookingDbContext context)
        {
            _context = context;
        }

        public void Create(T entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            _context.Set<T>().Add(entity);

        }

        public void Delete(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            _context.Set<T>().Remove(entity);

        }

        public IQueryable<T> FindAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>()
            .Where(expression).AsNoTracking();
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        public IQueryable<T> Query(string sqlquery)
        {
            return _context.Set<T>().FromSqlRaw(sqlquery);
        }
        public void CreateRange(List<T> entity)
        {
            foreach (var item in entity)
            {
                _context.Entry(item).State = EntityState.Added;
            }
            _context.Set<T>().AddRange(entity);
        }

        public void UpdateRange(List<T> entity)
        {
            foreach (var item in entity)
            {
                _context.Entry(item).State = EntityState.Modified;
            }
        }

        public void DeleteRange(List<T> entity)
        {
            foreach (var item in entity)
            {
                _context.Entry(item).State = EntityState.Deleted;
            }
            _context.Set<T>().RemoveRange(entity);
        }

        public IQueryable<T> Paging(int pageNumber, int pageSize, Expression<Func<T, bool>> orderby)
        {
            return _context.Set<T>().OrderBy(orderby).Skip((pageNumber - 1) * pageSize).Take(pageSize).AsNoTracking();
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<(List<T>, int)> PagingData(Expression<Func<T, bool>> query, Expression<Func<T, object>> sort, Expression<Func<T, object>> include, string sortOrder = "asc", int skip = 0, int take = 10)
        {
            var data = FindByCondition(query);
            if (include is not null)
                data = data.Include(include);
            if (sortOrder == "asc")
                return (await data.OrderBy(sort).Skip(skip).Take(take).ToListAsync(), await data.CountAsync());
            return (await data.OrderByDescending(sort).Skip(skip).Take(take).ToListAsync(), await data.CountAsync());
        }

        public async Task<int> Count(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().CountAsync(expression);
        }
    }
}
