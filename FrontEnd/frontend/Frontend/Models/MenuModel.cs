using System.Text.Json.Serialization;

namespace Frontend.Models
{
    public class MenuModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
        [JsonPropertyName("order")]
        public int Order { get; set; }
        [JsonPropertyName("controller")]
        public string Controller { get; set; } = string.Empty;
        [JsonPropertyName("attrs")]
        public List<MenuSiteAttr> LstMenuSiteAttr { get; set; } = new List<MenuSiteAttr>();
        [JsonPropertyName("is_submenu")]
        public bool isSubMenu
        {
            get
            {
                return LstMenuSiteAttr.Count > 0;
            }
        }
    }

    public class MenuSiteAttr
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("menu_id")]
        public int MenuId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("controller")]
        public string Controller { get; set; } = string.Empty;
        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;
        [JsonPropertyName("order")]
        public int Order { get; set; }
    }
}
