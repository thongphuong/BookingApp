using BookingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service.Interface
{
    public interface ISupplierRepository : IGenericRepository<Supplier>
    {
        public Task<List<SelectResponseDTO>> SelectSupplier(string query, int page);
        public IQueryable<BookingService.Domain.Supplier> GetByCondition(Expression<Func<BookingService.Domain.Supplier, bool>> expression);

        public Task<List<Supplier>> GetByConditionTask(Expression<Func<Supplier, bool>> expression);
    }
}
