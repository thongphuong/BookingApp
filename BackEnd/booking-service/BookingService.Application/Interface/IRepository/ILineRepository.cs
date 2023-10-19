using BookingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service.Interface
{
    public interface ILineRepository : IGenericRepository<Line>
    {
        public Task<List<SelectResponseDTO>> SelectLine(string query);

        public Task<List<Line>> GetByConditionLineTask(Expression<Func<Line, bool>> expression);



    }
}
