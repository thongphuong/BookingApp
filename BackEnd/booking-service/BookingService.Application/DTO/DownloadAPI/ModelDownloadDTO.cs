using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Service
{
    public class SupplierDownloadDTO
    {
        //[Required]
        [JsonPropertyName("supplier_code")]
        public string? Code { get; set; }

        //[Required]
        [JsonPropertyName("supplier_name")]
        public string? Name { get; set; }

        [JsonPropertyName("supplier_email")]
        public string? Email { get; set; }

        [JsonPropertyName("supplier_phone")]
        public string? Phone_Number { get; set; }

        [JsonPropertyName("supplier_status")]
        public int? Status { get; set; } // 0: InActive , 1:Active
    }
    public class LineDownloadDTO
    {
        //[Required]
        [JsonPropertyName("line_code")]
        public string? Code { get; set; }

        //[Required]
        [JsonPropertyName("line_name")]
        public string? Name { get; set; }

        [JsonPropertyName("line_status")]
        public int? Status { get; set; } // 0: InActive , 1:Active
    }

    public class DepartmentDownloadDTO
    {
        //[Required]
        [JsonPropertyName("line_code")]
        public string? line_Code { get; set; }

        //[Required]
        [JsonPropertyName("department_code")]
        public string? Code { get; set; }

        [Required]
        [JsonPropertyName("department_name")]
        public string? Name { get; set; }

        [JsonPropertyName("department_status")]
        public int? Status { get; set; } // 0: InActive , 1:Active
    }
    public class ProductDownloadDTO
    {
        //[Required]
        [JsonPropertyName("sku_code")]
        public string? Product_Code { get; set; }

        //[Required]
        [JsonPropertyName("product_name")]
        public string? Product_Name { get; set; }

        [JsonPropertyName("product_status")]
        public int? Status { get; set; } // 0: InActive , 1:Active
    }
}
