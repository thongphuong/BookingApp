using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Domain
{
    [Table("LINE_DEPARTMENT")]
    public class LineDepartment : BaseEntity
    {
        [Column("line_reference")]
        public Guid LineReference { get; set; }
        [Column("code")]
        public string? Code { get; set; }
        [Column("name")]
        public string? Name { get; set; }

        [Column("priority")]
        public bool? Priority { get; set; }

    }
}
