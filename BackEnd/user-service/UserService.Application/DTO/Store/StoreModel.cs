
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace UserService.Service.Store
{
   
    public partial class StoreModel
    {
        [JsonProperty(PropertyName = "id")]
        public int? id { get; set; }

        [JsonProperty(PropertyName = "reference_id")]
        public System.Guid reference_id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string? name { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string? address { get; set; }

        [JsonProperty(PropertyName = "phone_number")]
        public string? phone_number { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string? email { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string? code { get; set; }
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
        public string? status { get; set; }

        [JsonProperty(PropertyName = "ImageFile")]
        public List<IFormFile>? ImageFile { get; set; }

    }
}
