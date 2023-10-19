using BookingService.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookingService.Infrastructure
{
    public class ReturnDetailRepository : GenericRepository<Domain.ReturnDetail>, IReturnDetailRepository
    {
        public ReturnDetailRepository(BookingDbContext context) : base(context)
        {

        }
    }
}
