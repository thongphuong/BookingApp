using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class ReturnDetailModel
    {
        [JsonPropertyName("id")]
        public int? id { get; set; }

        [JsonPropertyName("product_code")]
        public string? product_code { get; set; }

        [JsonPropertyName("product_name")]
        public string? product_name { get; set; }

        [JsonPropertyName("number")]

        public int? number { get; set; }

        [JsonPropertyName("note")]

        public string? note { get; set; }

        [JsonPropertyName("return_reference")]
        public Guid? return_reference { get; set; }

        [JsonPropertyName("product_reference")]
        public Guid? product_reference { get; set; }

        [JsonPropertyName("reference_id")]
        public Guid? reference_id { get; set; }



    }
}
