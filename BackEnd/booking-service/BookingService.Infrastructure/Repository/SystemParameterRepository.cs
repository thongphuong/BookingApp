using BookingService.Domain;
using BookingService.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace BookingService.Infrastructure
{
    public class SystemParameterRepository : GenericRepository<SystemParameter>, ISystemParameterRepository
    {
        public SystemParameterRepository(BookingDbContext context) : base(context)
        {
        }
    }
}
