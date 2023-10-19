using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace BookingService.Service
{
    public class ReturnDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("line_name")]
        public string? Line_Name { get; set; } = string.Empty;

        [JsonPropertyName("line_code")]
        public string? Line_Code { get; set; } = string.Empty;

        [JsonPropertyName("department_name")]
        public string? Department_Name { get; set; } = string.Empty;

        [JsonPropertyName("department_code")]
        public string? Department_Code { get; set; } = string.Empty;

        [JsonPropertyName("supplier_name")]
        public string? Supplier_Name { get; set; } = string.Empty;

        [JsonPropertyName("store_name")]
        public string? Store_Name { get; set; } = string.Empty;

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("reference_id")]
        public Guid? Reference_Id { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }


    }
}
