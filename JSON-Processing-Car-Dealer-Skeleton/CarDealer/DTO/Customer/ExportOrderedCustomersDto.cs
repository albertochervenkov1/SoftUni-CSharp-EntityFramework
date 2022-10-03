using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CarDealer.DTO.Customer
{
    [JsonObject]
    public class ExportOrderedCustomersDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty(nameof(BirthDate))]

        public DateTime BirthDate { get; set; }

        [JsonProperty(nameof(IsYoungDriver))]
        public bool IsYoungDriver { get; set; }
    }
}
