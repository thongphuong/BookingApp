using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;
using UserService.Service.DTO.Return;
using UserService.Service.Interface;
using UserService.Service.Interface.IRepository;

namespace UserService.Infrastructure.Repository
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
