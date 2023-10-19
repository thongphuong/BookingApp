using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain
{
    [Table("MENU")]
    public class Menu : BaseEntity
    {
        [Column("name")]
        public string? Name { get; set; } = string.Empty;
        [Column("controller")]
        public string? Controller { get; set; } = string.Empty;
        [Column("icon")]
        public string? Icon { get; set; } = string.Empty;
        [Column("order")]
        public int? Order { get; set; }

        public IList<MenuAttribute> MenuAttributes { get; set; } = new List<MenuAttribute>();
    }
}
