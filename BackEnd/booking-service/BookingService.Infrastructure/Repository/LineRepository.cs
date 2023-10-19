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
    public class LineRepository : GenericRepository<Line>, ILineRepository
    {
        public LineRepository(BookingDbContext context) : base(context)
        {
        }

        public async Task<List<SelectResponseDTO>> SelectLine(string query)
        {
            return await FindByCondition(p => p.Status == (int)Domain.Enum.Status.Active && (string.IsNullOrEmpty(query) || p.Name.Contains(query))).Select(p => new SelectResponseDTO
            {
                Key = p.ReferenceId.ToString(),
                Value = p.Code + " - " + p.Name
            }).OrderBy(x => x.Value).ToListAsync();
        }
        public IQueryable<Line> GetByCondition(Expression<Func<Line, bool>> expression)
        {
            return FindByCondition(expression);
        }
        public async Task<List<Line>> GetByConditionLineTask(Expression<Func<Line, bool>> expression)
        {
            return await FindByCondition(expression).ToListAsync();
        }
    }
}
