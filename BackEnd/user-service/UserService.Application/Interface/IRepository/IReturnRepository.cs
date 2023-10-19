using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserService.Service.DTO.Return;

namespace UserService.Service.Interface.IRepository
{
    public interface IReturnRepository : IGenericRepository<ReturnDTO>
    {
        IQueryable<ReturnDTO> GetBySqlQuery(string sqlquery);
    }
}
