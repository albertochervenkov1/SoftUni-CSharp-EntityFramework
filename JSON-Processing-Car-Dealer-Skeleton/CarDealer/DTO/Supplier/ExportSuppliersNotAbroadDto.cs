using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CarDealer.DTO.Supplier
{
    [JsonObject]
    public class ExportSuppliersNotAbroadDto
    {
        [JsonProperty(nameof(Id))]
        public int Id { get; set; }

        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        [JsonProperty(nameof(PartsCount))]
        public int PartsCount { get; set; }
    }
}
