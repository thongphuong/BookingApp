using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UserService.Service
{
    public class UserDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("full_name")]
        public string? FullName { get; set; } = string.Empty;

        [JsonPropertyName("staff_id")]
        public string? StaffId { get; set; } = string.Empty;

        [JsonPropertyName("user_name")]
        public string? UserName { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string? Phone { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string? Email { get; set; } = string.Empty;

        [JsonPropertyName("role_id")]
        public int? RoleId { get; set; }
        [JsonPropertyName("role_name")]
        public string? RoleName { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string? Password { get; set; } = string.Empty;

        [JsonPropertyName("gender")]
        public string? Gender { get; set; } = string.Empty;

        [JsonPropertyName("division")]
        public string? Division { get; set; } = string.Empty;

        [JsonPropertyName("division_name")]
        public string? DivisionName { get; set; } = string.Empty;

        [JsonPropertyName("birthday")]
        public DateTime? Birthday { get; set; }

        [JsonPropertyName("department")]
        public string? Department { get; set; } = string.Empty;

        [JsonPropertyName("department_name")]
        public string? DepartmentName { get; set; } = string.Empty;

        [JsonPropertyName("working_location")]
        public Guid? WorkingLocation { get; set; }

        [JsonPropertyName("working_location_name")]
        public string? WorkingLocationName { get; set; } = string.Empty;

        [JsonPropertyName("store_management")]
        public Guid? StoreManagement { get; set; }

        [JsonPropertyName("store_management_name")]
        public string? StoreManagementName { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("reference_id")]

        public Guid? ReferenceId { get; set; }

    }
}
