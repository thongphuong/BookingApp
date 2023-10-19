using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Domain
{
    [Table("PRODUCT")]
    public class Product : BaseEntity
    {
        [Column("product_code")]
        [MaxLength(250)]
        public string? Product_Code { get; set; }

        [Column("product_name")]
        [MaxLength(500)]
        public string? Product_Name { get; set; }

    }
}
