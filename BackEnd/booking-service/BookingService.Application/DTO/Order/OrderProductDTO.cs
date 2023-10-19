using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class OrderProductDTO
    {
        [JsonPropertyName("product_reference")]
        public string? Product_Reference { get; set; }

        [JsonPropertyName("product_code")]
        public string? Product_Code { get; set; }

        [JsonPropertyName("product_name")]
        public string? Product_Name { get; set; }

        [JsonPropertyName("product_total_order")]
        public int? Product_Total_Order { get; set; }

        [JsonPropertyName("product_total_refuse")]
        public int? Product_Total_Refuse { get; set; }

        [JsonPropertyName("product_note")]
        public string? Product_Note { get; set; }

        [JsonPropertyName("product_image")]
        public List<IFormFile>? ImageFile { get; set; }

        [JsonPropertyName("image")]
        public string? Image { get; set; }

    }
}
