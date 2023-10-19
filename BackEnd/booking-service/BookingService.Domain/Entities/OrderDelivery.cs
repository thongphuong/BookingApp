using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Domain
{
    [Table("ORDER_DELIVERY")]
     public class OrderDelivery :BaseEntity
    {
        [Column("order_reference")]
        public System.Guid? Order_Reference { get; set; }

        [Column("code")]
        public string? Code  { get; set; }


    }
}
