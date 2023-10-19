using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UserService.Service
{
    public class RoleDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public int status { get; set; }

        [JsonPropertyName("functions")]
        public List<RoleDetailDTO> functions { get; set; } = new List<RoleDetailDTO>();

        [JsonPropertyName("functionstr")]
        public string FunctionStr { get; set; } = string.Empty;
    }


}
