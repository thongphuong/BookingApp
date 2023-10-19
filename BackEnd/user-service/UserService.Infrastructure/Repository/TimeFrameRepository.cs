using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;
using UserService.Service;
using UserService.Service.Interface;

namespace UserService.Infrastructure
{
    public class TimeFrameRepository : GenericRepository<TimeFrame>, ITimeFrameRepository
    {
        public TimeFrameRepository(BookingDbContext context) : base(context)
        {
        }
        public async Task<bool> IsExist(Expression<Func<TimeFrame, bool>> expression)
        {
            var isExist = await FirstOrDefault(expression);
            return isExist != null;
        }

        public async Task<List<SelectResponseDTO>> SelectTimeFrame(string query, Guid store)
        {
            return await FindByCondition(p => p.StoreReference == store && p.Status == (int)Domain.Enum.Status.Active && (string.IsNullOrEmpty(query) || p.TimeFrom.Contains(query) || p.TimeTo.Contains(query))).Select(p => new SelectResponseDTO
            {
                Key = p.ReferenceId.ToString(),
                Value = p.TimeFrom + " - " + p.TimeTo
            }).ToListAsync();
        }
    }
}
