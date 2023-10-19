using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Domain
{
    [Table("ROLE_DETAIL")]
    public class RoleDetail : BaseEntity
    {
        [Column("role_id")]
        public int? RoleId { get; set; }

        [Column("menu")]
        public string? Menu { get; set; } = string.Empty;

        [Column("sub_menu")]
        public string? SubMenu { get; set; } = string.Empty;

        [Column("controller")]
        public string? Controller { get; set; } = string.Empty;

        [Column("action")]
        public string? Action { get; set; } = string.Empty;

        public Role role { get; set; } = new Role();
    }
}