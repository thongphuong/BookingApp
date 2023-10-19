using Newtonsoft.Json;

namespace Frontend.Models
{
    public partial class StoreModel
    {
        public int id { get; set; }
        public System.Guid reference_id { get; set; }
        public string name { get; set; }
        public string app_name { get; set; }
        public string address { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public string ship { get; set; }
        public string target_gps { get; set; }
        public string url_image { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> deleted_at { get; set; }
        public Nullable<int> deleted_by { get; set; }
        public Nullable<System.DateTime> modify_at { get; set; }
        public Nullable<int> modify_by { get; set; }
        public string langcode { get; set; }
        public int status { get; set; }
        public string outlet { get; set; }
        public Nullable<int> code { get; set; }
        public string area_code { get; set; }
        public string url_image_bus { get; set; }
        public string manage { get; set; }
        public string apply { get; set; }
    }
}
