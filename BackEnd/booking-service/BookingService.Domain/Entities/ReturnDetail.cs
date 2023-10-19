using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Domain
{
    [Table("RETURN_DETAIL")]
    public class ReturnDetail : BaseEntity
    {

        [Column("product_reference")]
        public System.Guid? Product_Reference { get; set; }

        [Column("product_code")]
        public string? Product_Code { get; set; }

        [Column("product_name")]
        public string? Product_Name { get; set; }

        [Column("number")]

        public int? Number { get; set; }

        [Column("note")]

        public string? Note { get; set; }

        [Column("return_reference")]
        public System.Guid? Return_Reference { get; set; }

    }


}
