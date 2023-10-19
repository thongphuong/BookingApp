using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace BookingService.Service.Interface
{
    public interface IReturnRepository : IGenericRepository<ReturnDTO>
    {
        IQueryable<ReturnDTO> GetBySqlQuery(string sqlquery);
    }
}
