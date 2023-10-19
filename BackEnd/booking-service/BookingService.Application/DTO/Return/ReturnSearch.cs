using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public  class ReturnSearch
    {
        [JsonProperty(PropertyName = "referenceId")]
        public string? referenceId { get; set; }
    }
}
