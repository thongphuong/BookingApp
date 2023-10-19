using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Domain
{
    [Table("ORDER_PRODUCT")]
    public class OrderProduct : BaseEntity
    {
        [Column("order_reference")]
        public System.Guid? Order_Reference { get; set; }

        [Column("product_reference")]
        public string? Product_Reference { get; set; }

        [Column("product_code")]
        public string? Product_Code { get; set; }

        [Column("product_name")]
        public string? Product_Name { get; set; }

        [Column("product_total_order")]
        public int? Product_Total_Order { get; set; }

        [Column("product_total_refuse")]
        public int? Product_Total_Refuse { get; set; }

        [Column("product_note")]
        public string? Product_Note { get; set; }

        [Column("image")]
        public string? Image { get; set; }
    }
}
