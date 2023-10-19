using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UserService.Domain;

namespace UserService.Service.DTO.Return
{
    public class ReturnModel
    {
        [JsonPropertyName("store_reference")]
        public Guid? store_reference { get; set; }

        [JsonPropertyName("line_reference")]
        public Guid line_Reference { get; set; }

        [JsonPropertyName("department_reference")]
        public Guid department_Reference { get; set; }

        [JsonPropertyName("supplier_reference")]
        public Guid supplier_Reference { get; set; }

        [JsonPropertyName("supplier_name")]
        public string? supplier_Name { get; set; }

        [JsonPropertyName("date")]

        public DateTime? date { get; set; }

        [JsonPropertyName("time")]

        public DateTime? time { get; set; }
    }
}
