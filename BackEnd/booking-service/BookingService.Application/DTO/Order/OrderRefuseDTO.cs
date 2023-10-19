using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class OrderRefuseDTO
    {
        [JsonPropertyName("reference_id")]
        public System.Guid ReferenceId { get; set; }

        [JsonPropertyName("receiver_name")]
        public string? Receiver_Name { get; set; }

        [JsonPropertyName("receiver_department")]
        public string? Receiver_Department { get; set; }

        [JsonPropertyName("reason")]
        public string? Reason { get; set; }
        [JsonPropertyName("refuse_status")]
        public int? Refuse_Status { get; set; }

        [JsonPropertyName("receipt_status")]
        public int? Receipt_Status { get; set; }

        [JsonPropertyName("miss_invoice_number")]
        public int? Miss_Invoice_Number { get; set; }

        [JsonPropertyName("delivery_invoice_date")]
        public DateTime? Delivery_Invoice_Date { get; set; }

        [JsonProperty(PropertyName = "ImageFile")]
        public List<IFormFile>? ImageFile { get; set; }

        [JsonPropertyName("order_product")]
        public List<OrderProductDTO> Order_Product { get; set; } = new List<OrderProductDTO>();
    }
}
