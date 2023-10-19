using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class OrderDeliveryDTO
    {
        [JsonPropertyName("order_reference")]
        public System.Guid? Order_Reference { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }

    }
}
