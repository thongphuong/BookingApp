using BookingService.Domain;
using BookingService.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Infrastructure
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(BookingDbContext context) : base(context)
        {
        }
        public IQueryable<Order> GetByCondition(Expression<Func<Order, bool>> expression)
        {
            return FindByCondition(expression);
        }

        public async Task<bool> IsExist(Expression<Func<Order, bool>> expression)
        {
            var isExist = await FirstOrDefault(expression);
            return isExist != null;
        }
    }
}
