using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Service.Interface.IRepository;

namespace UserService.Infrastructure.Repository
{
    public class ReturnNewRepository : GenericRepository<Domain.Return>, IReturnNewRepository
    {
        public ReturnNewRepository(BookingDbContext context) : base(context)
        {

        }
    }
}
