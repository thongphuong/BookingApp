using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class SupplierDTO
    {
        [JsonPropertyName("supplier_code")]
        public string? Code { get; set; }

        [JsonPropertyName("supplier_name")]
        public string? Name { get; set; }

        [JsonPropertyName("supplier_email")]
        public string? Email { get; set; }

        [JsonPropertyName("supplier_phone")]
        public string? Phone_Number { get; set; }
    }
}
