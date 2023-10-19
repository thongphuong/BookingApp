using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain
{
    [Table("STORE")]
    public class Store
    {
        public int id { get; set; }
        public System.Guid reference_id { get; set; }
        [Required]
        public string? name { get; set; }
        [Required]
        public string? address { get; set; }
        [Required]
        public string? phone_number { get; set; }
        [Required]
        public string? email { get; set; }
        [Required]
        public string? url_image { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<int> created_by { get; set; }
        public Nullable<System.DateTime> deleted_at { get; set; }
        public Nullable<int> deleted_by { get; set; }
        public Nullable<System.DateTime> modify_at { get; set; }
        public Nullable<int> modify_by { get; set; }
        
        public int status { get; set; }
        [Required]
        public Nullable<int> code { get; set; }
    }
}
