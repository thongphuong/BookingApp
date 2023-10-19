using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class ProductDTO
    {
        [JsonPropertyName("product_code")]
        public string? Product_Code { get; set; }
        [JsonPropertyName("product_name")]
        public string? Product_Name { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
        [JsonPropertyName("created_by")]
        public int? CreatedBy { get; set; }
        [JsonPropertyName("modified_at")]
        public DateTime? ModifiedAt { get; set; }
        [JsonPropertyName("modified_by")]
        public int? ModifiedBy { get; set; }
        [JsonPropertyName("deleted_at")]
        public DateTime? DeletedAt { get; set; }
        [JsonPropertyName("deleted_by")]
        public int? DeteleBy { get; set; }
        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("reference_id")]
        public Guid? ReferenceId { get; set; }
    }
}
