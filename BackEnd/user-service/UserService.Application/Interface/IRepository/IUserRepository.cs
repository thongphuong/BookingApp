using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Service.Interface
{
    public interface IUserRepository : IGenericRepository<Domain.User>
    {
        Task<Domain.User> LoginUser(UserLogin user);
        Task<bool> IsExist(Expression<Func<UserService.Domain.User, bool>> expression);
    }
}
