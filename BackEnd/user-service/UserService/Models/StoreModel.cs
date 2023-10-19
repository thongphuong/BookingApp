
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace UserService.Models
{
   
    public partial class StoreModel
    {
        [JsonProperty(PropertyName = "id")]
        public int? id { get; set; }

        [JsonProperty(PropertyName = "reference_id")]
        public System.Guid reference_id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string? name { get; set; }

        [JsonProperty(PropertyName = "app_name")]
        public string? app_name { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string? address { get; set; }
        [JsonProperty(PropertyName = "phone_number")]

        public string? phone_number { get; set; }
        [JsonProperty(PropertyName = "email")]

        public string? email { get; set; }
        [JsonProperty(PropertyName = "ship")]
        public string? ship { get; set; }

        [JsonProperty(PropertyName = "target_gps")]
        public string? target_gps { get; set; }

        [JsonProperty(PropertyName = "url_image")]
        public string? url_image { get; set; }
        [JsonProperty(PropertyName = "created_at")]

        public Nullable<System.DateTime> created_at { get; set; }
        [JsonProperty(PropertyName = "created_by")]

        public Nullable<int> created_by { get; set; }
        [JsonProperty(PropertyName = "deleted_at")]

        public Nullable<System.DateTime> deleted_at { get; set; }

        [JsonProperty(PropertyName = "deleted_by")]
        public Nullable<int> deleted_by { get; set; }

        [JsonProperty(PropertyName = "modify_at")]
        public Nullable<System.DateTime> modify_at { get; set; }

        [JsonProperty(PropertyName = "modify_by")]
        public Nullable<int> modify_by { get; set; }

        [JsonProperty(PropertyName = "langcode")]
        public string? langcode { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int status { get; set; }

        [JsonProperty(PropertyName = "outlet")]
        public string? outlet { get; set; }

        [JsonProperty(PropertyName = "code")]

        public Nullable<int> code { get; set; }

        [JsonProperty(PropertyName = "area_code")]
        public string? area_code { get; set; }

        [JsonProperty(PropertyName = "url_image_bus")]
        public string? url_image_bus { get; set; }

        [JsonProperty(PropertyName = "manage")]
        public string? manage { get; set; }

        [JsonProperty(PropertyName = "apply")]

        public string? apply { get; set; }

        [JsonProperty(PropertyName = "ImageFile")]
        public List<IFormFile>? ImageFile { get; set; }

    }
}
