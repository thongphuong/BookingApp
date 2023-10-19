using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UserService.Service
{
    public class FunctionDTO
    {
        [JsonPropertyName("menu")]
        public string? Menu { get; set; } = string.Empty;
        [JsonPropertyName("submenu")]
        public string? Submenu { get; set; } = string.Empty;
        [JsonPropertyName("function")]
        public string? Funtion { get; set; } = string.Empty;
        [JsonPropertyName("controller")]
        public string? Controller { get; set; } = string.Empty;
        [JsonPropertyName("action")]
        public string? Action { get; set; } = string.Empty;
    }
}
