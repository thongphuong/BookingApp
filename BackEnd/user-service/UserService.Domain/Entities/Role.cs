using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain
{
    [Table("ROLE")]
    public class Role : BaseEntity
    {
        [Column("name")]
        public string? Name { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; } = string.Empty;

        public IList<RoleDetail> RoleDetails { get; set; } = new List<RoleDetail>();
        public IList<User> Users { get; set; } = new List<User>();
    }
}
