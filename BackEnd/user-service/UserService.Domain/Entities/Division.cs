using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UserService.Domain
{
    [Table("DIVISIONS")]
    public class Divisions : BaseEntity
    {
        [JsonPropertyName("department_id")]
        public int? DepartmentID { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        public Department Deparment { get; set; } = new Department();


    }
}
