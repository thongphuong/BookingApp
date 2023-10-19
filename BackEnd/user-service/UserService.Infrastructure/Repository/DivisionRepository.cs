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
    public class DivisionRepository : GenericRepository<Divisions>, IDivisionRepository
    {
        public DivisionRepository(BookingDbContext context) : base(context)
        {
        }

        public async Task<List<SelectResponseDTO>> SelectDivision(string query, Guid department)
        {

            return await FindByCondition(p => p.Status == (int)Domain.Enum.Status.Active && (string.IsNullOrEmpty(query) || p.Name.Contains(query))).Include(p => p.Deparment).Where(p => department == Guid.Empty || p.Deparment.ReferenceId == department).AsNoTracking().Select(p => new SelectResponseDTO
            {
                Key = p.ReferenceId.ToString(),
                Value = p.Name
            }).ToListAsync();
        }
    }
}
