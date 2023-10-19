using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;

namespace UserService.Service.Interface
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {
        public Task<List<Menu>> GetRoleMenuOrderByAsync(Expression<Func<Menu, bool>> expression, List<string> subMenus);
    }
}
