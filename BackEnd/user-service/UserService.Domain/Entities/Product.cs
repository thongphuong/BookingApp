using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain
{
    [Table("PRODUCT")]
    public class Product : BaseEntity
    { 
        [Column("product_code")]
        public string? Product_Code { get; set; }

        [Column("product_name")]
        public string? Product_Name { get; set; }

    }
}
