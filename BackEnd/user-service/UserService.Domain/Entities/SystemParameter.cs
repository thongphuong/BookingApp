using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UserService.Domain
{
    [Table("SYSTEM_PARAMETER")]
    public class SystemParameter : BaseEntity
    {
        [Column("parameter_name")]
        public string ParameterName { get; set; } = string.Empty;
        [Column("value")]
        public string? Value { get; set; } = string.Empty;
        [Column("note")]
        public string? Note { get; set; } = string.Empty;
    }
}
