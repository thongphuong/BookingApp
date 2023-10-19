using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class ReturnDetailDTO
    {
        [JsonPropertyName("product_code")]
        public string? Product_Code { get; set; }

        [JsonPropertyName("product_name")]
        public string? Product_Name { get; set; }

        [JsonPropertyName("number")]

        public int? Number { get; set; }

        [JsonPropertyName("note")]

        public string? Note { get; set; }

        [JsonPropertyName("return_reference")]
        public System.Guid Return_Reference { get; set; }
    }
}
