using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Service
{
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
