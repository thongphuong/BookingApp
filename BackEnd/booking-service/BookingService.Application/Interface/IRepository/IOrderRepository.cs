using BookingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service.Interface
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public IQueryable<BookingService.Domain.Order> GetByCondition(Expression<Func<BookingService.Domain.Order, bool>> expression);
        public Task<bool> IsExist(Expression<Func<Order, bool>> expression);
    }
}
