using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Domain
{
    [Table("USER")]
    public class User : BaseEntity
    {
        [Column("full_name")]
        [Required]
        public string? FullName { get; set; } = string.Empty;

        [Column("staff_id")]
        [Required]
        public string? StaffId { get; set; } = string.Empty;

        [Column("user_name")]
        [Required]
        public string? UserName { get; set; } = string.Empty;

        [Column("phone")]
        [Required]
        public string? Phone { get; set; } = string.Empty;

        [Column("email")]
        public string? Email { get; set; } = string.Empty;

        [Column("role_id")]
        [Required]
        public int? RoleId { get; set; }

        [Column("password")]
        [Required]
        public string? Password { get; set; } = string.Empty;

        [Column("gender")]
        public string? Gender { get; set; } = string.Empty;

        [Column("division")]
        public string? Division { get; set; } = string.Empty;

        [Column("birthday")]
        public DateTime? Birthday { get; set; }

        [Column("department")]
        public string? Department { get; set; } = string.Empty;

        [Column("working_location")]
        public Guid? WorkingLocation { get; set; }

        [Column("store_management")]
        public Guid? StoreManagement { get; set; }

        [Column("last_online_at")]
        public DateTime? LastOnline { get; set; }
    }
}
