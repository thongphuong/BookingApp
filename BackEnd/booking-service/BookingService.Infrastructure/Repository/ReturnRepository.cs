using BookingService.Service;
using BookingService.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Infrastructure
{
    public class ReturnRepository : GenericRepository<ReturnDTO>, IReturnRepository
    {
        public ReturnRepository(BookingDbContext context) : base(context)
        {
            
        }
        public IQueryable<ReturnDTO> GetBySqlQuery(string sqlquery)
        {
            var result = Query(sqlquery);
            return result;
        }
       
    }
}
