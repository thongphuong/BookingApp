using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class HistoryDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("time")]
        public DateTime? Time { get; set; }

        [JsonPropertyName("supplier_code")]
        public string? Supplier_Code { get; set; }

        [JsonPropertyName("supplier_name")]
        public string? Supplier_Name { get; set; }

        [JsonPropertyName("qr_code")]
        public string? QR_Code { get; set; }

        [JsonPropertyName("code")]

        public string? Code { get; set; }

        [JsonPropertyName("user")]
        public int? User { get; set; }

        [JsonPropertyName("status")]
        public int? Status { get; set; }
    }
}
