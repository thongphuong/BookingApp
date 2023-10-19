using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain
{
    [Table("RETURN")]
    public class Return : BaseEntity
    { 
        [Column("store_reference")]
        public System.Guid Store_Reference { get; set; }

        [Column("line_reference")]
        public Guid Line_Reference { get; set; }

        [Column("department_reference")]
        public Guid Department_Reference { get; set; }

        [Column("supplier_reference")]
        public Guid Supplier_Reference { get; set; }

        [Column("supplier_name")]
        public string? Supplier_Name { get; set; }

        [Column("date")]

        public DateTime? Date { get; set; }

        [Column("time")]

        public DateTime? Time { get; set; }

    }
}
