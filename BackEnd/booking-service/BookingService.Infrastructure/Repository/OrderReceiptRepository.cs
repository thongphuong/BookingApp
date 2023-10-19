using BookingService.Domain;
using BookingService.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Infrastructure
{
    public class OrderReceiptRepository : GenericRepository<OrderReceipt>, IOrderReceiptRepository
    {
        public OrderReceiptRepository(BookingDbContext context) : base(context)
        {
        }
    }
}
