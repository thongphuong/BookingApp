using BookingService.Domain;
using BookingService.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Infrastructure
{
    public class OrderDeliveryRepository : GenericRepository<OrderDelivery>, IOrderDeliveryRepository
    {
        public OrderDeliveryRepository(BookingDbContext context) : base(context)
        {
        }
    }
}
