using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BookingService.Domain
{
    [Table("DEPARTMENT")]
    public class Department : BaseEntity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        public IList<Divisions> Divisions { get; set; } = new List<Divisions>();

    }
}
