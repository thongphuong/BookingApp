using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Service.Interface.IRepository;

namespace UserService.Infrastructure.Repository
{
    public class ReturnDetailRepository : GenericRepository<Domain.ReturnDetail>, IReturnDetailRepository
    {
        public ReturnDetailRepository(BookingDbContext context) : base(context)
        {

        }
    }
}
