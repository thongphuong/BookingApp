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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(BookingDbContext context) : base(context)
        {
        }

        public async Task<User> LoginUser(UserLogin user)
        {
            return await FindByCondition(p => p.UserName == user.UserName && p.Password == user.Password && p.Status == (int)Domain.Enum.Status.Active).Include(p => p.role).FirstOrDefaultAsync() ?? new User();
        }

        public async Task<bool> IsExist(Expression<Func<User, bool>> expression)
        {
            var isExist = await FirstOrDefault(expression);
            return isExist != null;
        }
    }
}
