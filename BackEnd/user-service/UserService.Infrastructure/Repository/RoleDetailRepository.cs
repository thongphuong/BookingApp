using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;
using UserService.Service.Interface;

namespace UserService.Infrastructure
{
    internal class RoleDetailRepository : GenericRepository<RoleDetail>, IRoleDetailRepository
    {
        public RoleDetailRepository(BookingDbContext context) : base(context)
        {
        }

        public async Task<List<RoleDetail>> GetRoleDetaisById(int roleId)
        {
            return await FindByCondition(p => p.RoleId == roleId && p.Status != (int)Domain.Enum.Status.Delete).ToListAsync();
        }
    }
}
