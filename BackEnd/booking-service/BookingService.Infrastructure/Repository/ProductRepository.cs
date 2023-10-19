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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(BookingDbContext context) : base(context)
        {

        }

        public async Task<List<Product>> GetByConditionTask(Expression<Func<Product, bool>> expression)
        {
            return await FindByCondition(expression).ToListAsync();
        }

        public async Task<List<SelectResponseDTO>> SelectProduct(string query, int page)
        {
            return await FindByCondition(p => p.Status == (int)Domain.Enum.Status.Active && (string.IsNullOrEmpty(query) || p.Product_Name.Contains(query))).Select(p => new SelectResponseDTO
            {
                Key = p.ReferenceId.ToString(),
                Value = p.Product_Code.ToString(),
                listAttr = new List<SelectAttr>() { new SelectAttr
                {
                    Key ="product_name",
                    Value = p.Product_Name
                }}
            }).Skip(page).Take(page * 100).ToListAsync();
        }
        public async Task<List<SelectResponseDTO>> SelectProductPaging(string query,int skip,int top)
        {
            var count = FindAll().Count();
            return await FindByCondition(p => p.Status == (int)Domain.Enum.Status.Active && (string.IsNullOrEmpty(query) || p.Product_Name.Contains(query))).Select(p => new SelectResponseDTO
            {
                Key = p.ReferenceId.ToString(),
                Value = p.Product_Code.ToString(),
                Total = count,
                listAttr = new List<SelectAttr>() { new SelectAttr
                {
                    Key ="product_name",
                    Value = p.Product_Name
                }}
            }).Skip(skip).Take(top).ToListAsync();
        }
    }
}
