using System.Text.Json.Serialization;

namespace Frontend.Models
{
    public class CurrentUserModel
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("reference_id")]
        public Guid? ReferenceId { get; set; }
        [JsonPropertyName("user_name")]
        public string FullName { get; set; } = string.Empty;
        [JsonPropertyName("full_name")]
        public string UserName { get; set; } = string.Empty;
        [JsonPropertyName("role_id")]
        public int? RoleId { get; set; }
        [JsonPropertyName("role_name")]
        public string RoleName { get; set; } = string.Empty;
        [JsonPropertyName("first_login")]
        public bool FirstLogin { get; set; }
        [JsonPropertyName("role_detail")]
        public List<RoleDetailDTO> roleDetailDTOs { get; set; } = new List<RoleDetailDTO>();
    }

    public class RoleDetailDTO
    {
        [JsonPropertyName("role_id")]
        public int RoleId { get; set; }
        [JsonPropertyName("menu")]
        public string Menu { get; set; } = string.Empty;
        [JsonPropertyName("sub_menu")]
        public string SubMenu { get; set; } = string.Empty;
        [JsonPropertyName("controller")]
        public string Controller { get; set; } = string.Empty;
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;
        [JsonPropertyName("status")]
        public int Status { get; set; }
    }
}
