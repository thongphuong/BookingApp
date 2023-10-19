using BookingService.Domain;
using BookingService.Service;
using BookingService.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookingService.Infrastructure
{
    public class SupplierRepository : GenericRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(BookingDbContext context) : base(context)
        {
        }

        public async Task<List<SelectResponseDTO>> SelectSupplier(string query, int page)
        {
            return await FindByCondition(p => p.Status == (int)Domain.Enum.Status.Active && (string.IsNullOrEmpty(query) || p.Code.Contains(query)) || p.Name.Contains(query)).Select(p => new SelectResponseDTO
            {
                Key = p.ReferenceId.ToString(),
                Value = p.Code
            }).Skip(page).Take(page * 100).ToListAsync();
        }

        public IQueryable<Supplier> GetByCondition(Expression<Func<Supplier, bool>> expression)
        {
            return FindByCondition(expression);
        }

        public async Task<List<Supplier>> GetByConditionTask(Expression<Func<Supplier, bool>> expression)
        {
            return await FindByCondition(expression).ToListAsync();
        }
    }
}
