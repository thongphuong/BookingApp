using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Domain
{
    [Table("LINE")]
    public class Line : BaseEntity
    {
        [Column("code")]
        public string? Code { get; set; }

        [Column("name")]
        public string? Name { get; set; }
    }
}
