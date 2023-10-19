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
    public class SystemParameterRepository : GenericRepository<SystemParameter>, ISystemParameterRepository
    {
        public SystemParameterRepository(BookingDbContext context) : base(context)
        {
        }
    }
}
