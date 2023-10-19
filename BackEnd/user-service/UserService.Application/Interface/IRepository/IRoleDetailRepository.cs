using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;

namespace UserService.Service.Interface
{
    public interface IRoleDetailRepository : IGenericRepository<RoleDetail>
    {
        public Task<List<RoleDetail>> GetRoleDetaisById(int roleId);
    }
}
