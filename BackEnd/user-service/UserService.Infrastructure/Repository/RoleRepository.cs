using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;
using UserService.Service;
using UserService.Service.Interface;

namespace UserService.Infrastructure
{
    internal class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(BookingDbContext context) : base(context)
        {
        }

        public async Task<List<SelectResponseDTO>> DropDownRole(string query)
        {
            return await FindByCondition(p => p.Status == (int)Domain.Enum.Status.Active && (string.IsNullOrEmpty(query) || p.Name.Contains(query))).Select(p => new SelectResponseDTO
            {
                Key = p.Id.ToString(),
                Value = p.Name
            }).ToListAsync();
        }
    }
}
