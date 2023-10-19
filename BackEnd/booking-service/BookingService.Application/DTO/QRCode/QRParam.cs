using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class QRParam
    {

        [JsonProperty(PropertyName = "qr_code")]
        public string? QR_Code { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string? Code { get; set; }
    }
}
