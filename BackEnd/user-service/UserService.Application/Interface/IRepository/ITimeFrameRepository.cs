using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Service.Interface
{
    public interface ITimeFrameRepository : IGenericRepository<Domain.TimeFrame>
    {
        Task<bool> IsExist(Expression<Func<Domain.TimeFrame, bool>> expression);
        public Task<List<SelectResponseDTO>> SelectTimeFrame(string query, Guid store);
    }
}
