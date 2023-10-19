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

namespace BookingService.Infrastructure
{
    public class StoreRepository : GenericRepository<Store>, IStoreRepository
    {
        public StoreRepository(BookingDbContext context) : base(context)
        {
        }
        public async Task<List<SelectResponseDTO>> SelectStore(string query)
        {
            return await FindByCondition(p => p.status == (int)Domain.Enum.Status.Active && (string.IsNullOrEmpty(query) || p.name.Contains(query))).Select(p => new SelectResponseDTO
            {
                Key = p.reference_id.ToString(),
                Value = p.name
            }).ToListAsync();
        }

        public async Task<List<Store>> GetListStore(Expression<Func<Domain.Store, bool>> expression)
        {
            return await FindByCondition(expression).ToListAsync();
        }
        public IQueryable<Store> GetByCondition(Expression<Func<Store, bool>> expression)
        {
            return FindByCondition(expression);
        }
    }
}
