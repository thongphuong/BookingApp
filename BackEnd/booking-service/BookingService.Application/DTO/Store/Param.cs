using Newtonsoft.Json;

namespace UserService.Service.Store
{
    public partial class Param
    {
        [JsonProperty(PropertyName = "referenceId")]
        public string? referenceId { get; set; }
    }
}
