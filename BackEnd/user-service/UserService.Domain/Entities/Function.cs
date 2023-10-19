using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain
{
    [Table("FUNCTION")]
    public class Function : BaseEntity
    {
        [Column("menu")]
        public string? Menu { get; set; } = string.Empty;
        [Column("sub_menu")]
        public string? Submenu { get; set; } = string.Empty;
        [Column("function")]
        public string? Funtion { get; set; } = string.Empty;
        [Column("controller")]
        public string? Controller { get; set; } = string.Empty;
        [Column("action")]
        public string? Action { get; set; } = string.Empty;
        [Column("order")]
        public string? Order { get; set; } = string.Empty;
    }
}
