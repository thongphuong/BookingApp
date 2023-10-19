using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UserService.Domain;

namespace UserService.Service.DTO.Return
{
    public class ReturnDetailModel
    {
        [JsonPropertyName("product_code")]
        public string? product_code { get; set; }

        [JsonPropertyName("product_name")]
        public string? product_name { get; set; }

        [JsonPropertyName("number")]

        public int? number { get; set; }

       [JsonPropertyName("note")]

        public string? note { get; set; }

        [JsonPropertyName("return_reference")]
        public string? return_reference { get; set; }
    }
}
