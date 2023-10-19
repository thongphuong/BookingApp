using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain;
using UserService.Service.Interface;

namespace UserService.Infrastructure
{
    public class MenuAttrRepository : GenericRepository<MenuAttribute>, IMenuAttrRepository
    {
        public MenuAttrRepository(BookingDbContext context) : base(context)
        {
        }
    }
}
