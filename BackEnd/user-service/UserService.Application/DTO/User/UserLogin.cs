using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UserService.Service
{
    public class UserLogin
    {
        [JsonPropertyName("user_name")]
        public string UserName { get; set; } = string.Empty;
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
    public class UserLoginResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("reference_id")]
        public Guid? reference_id { get; set; }

        [JsonPropertyName("user_name")]
        public string UserName { get; set; } = string.Empty;
        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = string.Empty;
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
        [JsonPropertyName("role")]
        public int? Role { get; set; }
        [JsonPropertyName("role_name")]
        public string RoleName { get; set; } = string.Empty;

        [JsonPropertyName("division")]
        public string Devision { get; set; } = string.Empty;

        [JsonPropertyName("first_login")]
        public bool FirstLogin { get; set; }

        [JsonPropertyName("role_detail")]
        public List<RoleDetailDTO> roleDetailDTOs { get; set; } = new List<RoleDetailDTO>();
    }
}
