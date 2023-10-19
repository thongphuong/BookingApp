using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Domain
{

    [Table("SUPPLIER")]
    public class Supplier : BaseEntity
    {
        [Required]
        [Column("supplier_code")]
        [MaxLength(250)]
        public string? Code { get; set; }

        [Column("supplier_name")]
        [MaxLength(500)]
        public string? Name { get; set; }

        [Column("supplier_email")]
        public string? Email { get; set; }

        [Column("supplier_phone")]
        public string? Phone_Number { get; set; }
    }
}
