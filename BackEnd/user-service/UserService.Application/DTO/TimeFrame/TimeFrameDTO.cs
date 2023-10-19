using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UserService.Service
{
    public class TimeFrameDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("store_ref")]
        public Guid? StoreReference { get; set; } = Guid.Empty;
        [JsonPropertyName("store_code")]
        public string? StoreCode { get; set; } = string.Empty;

        [JsonPropertyName("store_name")]
        public string? StoreName { get; set; } = string.Empty;

        [JsonPropertyName("time_from")]
        public string? TimeFrom { get; set; } = string.Empty;

        [JsonPropertyName("time_to")]
        public string? TimeTo { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public int? Amount { get; set; }

        [JsonPropertyName("status")]
        public int? Status { get; set; }

        [JsonPropertyName("reference_id")]
        public Guid? ReferenceId { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

    }
}
