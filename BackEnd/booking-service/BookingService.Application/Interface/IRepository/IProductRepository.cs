using BookingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service.Interface
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public Task<List<SelectResponseDTO>> SelectProduct(string query, int page);
        public Task<List<SelectResponseDTO>> SelectProductPaging(string query, int skip, int top);
        public Task<List<Product>> GetByConditionTask(Expression<Func<Product, bool>> expression);

    }
}
