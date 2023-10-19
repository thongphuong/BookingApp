using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Domain
{
    [Table("MENU_ATTRIBUTE")]
    public class MenuAttribute : BaseEntity
    {
        [Column("menu_id")]
        public int? MenuId { get; set; }
        [Column("name")]
        public string? Name { get; set; } = string.Empty;
        [Column("controller")]
        public string? Controller { get; set; } = string.Empty;
        [Column("action")]
        public string? Action { get; set; } = string.Empty;
        [Column("order")]
        public int? Order { get; set; }
        public Menu menu { get; set; } = new Menu();

    }
}