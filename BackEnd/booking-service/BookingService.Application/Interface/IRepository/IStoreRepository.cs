using BookingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service.Interface
{
    public interface IStoreRepository : IGenericRepository<Store>
    {
        public Task<List<SelectResponseDTO>> SelectStore(string query);

        public Task<List<Domain.Store>> GetListStore(Expression<Func<Domain.Store, bool>> expression);
        public IQueryable<BookingService.Domain.Store> GetByCondition(Expression<Func<BookingService.Domain.Store, bool>> expression);
    }
}
