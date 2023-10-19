using BookingService.Domain;
using BookingService.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Infrastructure.Repository
{
    public class HistoryRepository : GenericRepository<History>,IHistoryRepository
    {
        public HistoryRepository(BookingDbContext context) : base(context)
        {

        }
        public IQueryable<History> GetByCondition(Expression<Func<History, bool>> expression)
        {
            return FindByCondition(expression);
        }
    }
}
