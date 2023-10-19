using Newtonsoft.Json;

namespace UserService.Models
{
    public partial class Param
    {
        [JsonProperty(PropertyName = "referenceId")]
        public string? referenceId { get; set; }
    }
}
