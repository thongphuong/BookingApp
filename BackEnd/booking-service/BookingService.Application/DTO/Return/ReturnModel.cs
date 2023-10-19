using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class ReturnModel
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("store_reference")]
        public Guid? Store_Reference { get; set; }

        [JsonPropertyName("store_name")]
        public string? Store_Name { get; set; }

        [JsonPropertyName("line_reference")]
        public Guid? Line_Reference { get; set; }

        [JsonPropertyName("line_name")]
        public string? Line_Name { get; set; }

        [JsonPropertyName("department_reference")]
        public Guid? Department_Reference { get; set; }

        [JsonPropertyName("department_name")]
        public string?Department_Name { get; set; }

        [JsonPropertyName("supplier_reference")]
        public Guid? Supplier_Reference { get; set; }

        [JsonPropertyName("supplier_code")]
        public string? Supplier_Code { get; set; }

        [JsonPropertyName("supplier_name")]
        public string? Supplier_Name { get; set; }

        [JsonPropertyName("date")]

        public DateTime? Date { get; set; }

        [JsonPropertyName("time")]

        public TimeSpan? Time { get; set; }

        [JsonPropertyName("reference_id")]

        public Guid? Reference_Id {get; set;}
}
}
