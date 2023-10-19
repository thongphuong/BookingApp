using BookingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service.Interface
{
    public interface ILineDepartmentRepository : IGenericRepository<LineDepartment>
    {
        public Task<List<SelectResponseDTO>> SelectLineDepartment(string query, Guid line);

        public Task<List<LineDepartment>> GetByConditionDepartmentTask(Expression<Func<LineDepartment, bool>> expression);

    }
}
