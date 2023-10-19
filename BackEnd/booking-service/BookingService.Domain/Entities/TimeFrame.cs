using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Domain
{
    [Table("TIME_FRAME")]
    public class TimeFrame : BaseEntity
    {
        [Column("store_ref")]
        public Guid? StoreReference { get; set; } = Guid.Empty;

        [Column("time_from")]
        public string? TimeFrom { get; set; } = string.Empty;

        [Column("time_to")]
        public string? TimeTo { get; set; } = string.Empty;

        [Column("amount")]
        public int? Amount { get; set; }
    }
}
