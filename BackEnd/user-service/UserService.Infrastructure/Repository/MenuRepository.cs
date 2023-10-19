using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;
using UserService.Service.Interface;

namespace UserService.Infrastructure
{
    public class MenuRepository : GenericRepository<Menu>, IMenuRepository
    {
        public MenuRepository(BookingDbContext context) : base(context)
        {
        }

        public async Task<List<Menu>> GetRoleMenuOrderByAsync(Expression<Func<Menu, bool>> expression, List<string> subMenus)
        {
            return await FindByCondition(expression).OrderBy(p => p.Order).Include(p => p.MenuAttributes.Where(x => subMenus.Contains(x.Name))).ToListAsync();
        }
    }
}
