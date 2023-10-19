using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Domain
{
    [Table("HISTORY")]
    public class History : BaseEntity
    {

        [Column("time")]
        public DateTime? Time { get; set; }

        [Column("supplier_code")]
        public string? Supplier_Code { get; set; }

        [Column("qr_code")]
        public string? QR_Code { get; set; }

        [Column("code")]

        public string? Code { get; set; }

    }
}
